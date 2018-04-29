using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStoreDemo.Api.Controllers;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Genre;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Common.Repositories;
using Xunit;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Models.Album;

namespace MusicStoreDemo.Tests.Api.Repositories
{
    [Trait("Category", "RepositoryTests")]
    public class AlbumRepositoryTests : IDisposable
    {
        private AlbumRepository repo;
        private readonly MusicStoreDbContext db;
        private readonly string[] _validGenres = { "genre_1", "genre_2", "genre_3","genre_4", "genre_5", "genre_6" };
        private int _validImageId;
        private int _validArtistId;

        public AlbumRepositoryTests()
        {
            // set up test data
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db" + Guid.NewGuid().ToString())
                .Options;
            this.db = new MusicStoreDbContext(options);
            foreach (string g in _validGenres)
            {
                this.db.Genres.Add(new DbGenre { Name = g, CreatedUtc = DateTime.UtcNow });
            }
            var imageResource = new DbImageResource(){ 
                MimeType = "img/png",
                Data = new byte[10]
            };
            var artist = new DbArtist
            {
                BioText = "",
                CreatedUtc = DateTime.UtcNow,
                Name = "test artist",
                PublishStatus = DbPublishedStatus.PUBLISHED,
                UpdatedUtc = DateTime.UtcNow
            };
            db.ImageResources.Add(imageResource);
            db.Artists.Add(artist);

            db.SaveChanges();
            _validImageId = imageResource.Id;
            _validArtistId = artist.Id;
            var loggerMock = new Mock<ILogger<ArtistController>>();
            
            this.repo = new AlbumRepository(this.db, new AlbumMapper());
            
        }

        public void Dispose()
        {
            // tear down in memory db after each test
            this.db.Database.EnsureDeleted();
            this.db.Dispose();
        }

        // returns a valid object model that should succeed when calling the api
        private Album CreateValidModel(){
            var album = new Album()
            {
                Title = "test artist",
                Genres = new List<string> { "genre_1" },
                DescriptionText = "Artist text bio",
                CoverImageId = _validImageId,
                PublishedStatus = PublishStatus.UNPUBLISHED,
                ArtistId = _validArtistId,
                ArtistName = "artist name",
                Label = "Label 1",
                Price = 10.99,
                Producer = "Producer 1",
                ReleaseDate = new DateTime(1999, 1, 1),
                TotalDurationInSeconds = 0,
                
            };
            for(int i = 0; i < 10; i++)
            {
                int duration = 120;
                album.Tracks.Add(new AlbumTrack
                {
                    DurationInSeconds = duration,
                    Title = $"track # {i}",
                    TrackNumber = i
                });
                album.TotalDurationInSeconds += duration;
            }
            
            return album;
        }

        // sets up a valid album in the database for get/update tests
        private async Task<int> SetupValidAlbumRecordInDatabase(){
            // create code is tested in other unit tests
            var result = await this.repo.AddAsync(CreateValidModel());
            return result.Id;
        }

        [Fact]
        public async void AddAlbum_ReturnsNonNullResult_WithValidInput(){
            Album input = CreateValidModel();
            AlbumDetail result = await this.repo.AddAsync(input);
            Assert.NotNull(result);
        }


        [Fact]
        public async void AddAlbum_ReturnsAlbumDetailResult_WithValidInput()
        {
            Album input = CreateValidModel();
            AlbumDetail result = await this.repo.AddAsync(input);
           
            Assert.IsType<AlbumDetail>(result);
        }

        [Fact]
        public async void AddAlbum_ThrowsRepoException_WhenInvalidGenreIsSupplied()
        {
            Album input = CreateValidModel();
            // chenge input to have invalid genres
            input.Genres = new List<string> { "invalidGenreName_534546" };
            await Assert.ThrowsAsync<RepositoryException>( () => this.repo.AddAsync(input) );
        }

        [Fact]
        public async void AddAlbum_ReturnsResultWithMatchingProperties()
        {
            Album input = CreateValidModel();
            AlbumDetail createdObj = await this.repo.AddAsync(input);

            Assert.Equal(input.Title, createdObj.Title);
            Assert.Equal(input.PublishedStatus, createdObj.PublishedStatus);
            Assert.Equal(input.DescriptionText, createdObj.DescriptionText);
            Assert.Equal(input.Label, createdObj.Label);
            Assert.Equal(input.Price, createdObj.Price);
            Assert.Equal(input.Producer, createdObj.Producer);
            Assert.Equal(input.ReleaseDate, createdObj.ReleaseDate);
            Assert.Equal(input.TotalDurationInSeconds, createdObj.TotalDurationInSeconds);
            Assert.Equal(input.Genres, createdObj.Genres);
        }

        [Fact]
        public async void AddAlbum_ReturnsResultWithCorrectNumberOfTracks()
        {
            Album input = CreateValidModel();
            AlbumDetail createdObj = await this.repo.AddAsync(input);

            Assert.Equal(input.Tracks.Count, createdObj.Tracks.Count);
            
        }

        [Fact]
        public async void GetAlbum_ReturnsNullWhenNoArtistExists()
        {
            AlbumDetail result = await this.repo.GetAsync(929292);
            Assert.Null(result);
        }

        [Fact]
        public async void GetAlbum_ReturnsTracksWithValidIds()
        {
            int albumId = await SetupValidAlbumRecordInDatabase();
            AlbumDetail result = await this.repo.GetAsync(albumId);
            foreach(var t in result.Tracks)
            {
                Assert.NotNull(t.TrackId);
            }
        }

        [Fact]
        public async void UpdateAlbum_ReturnsResultWithMatchingProperties()
        {
            int existingAlbumId = await SetupValidAlbumRecordInDatabase();
            Album updateModel = new Album
            {
                Genres = new List<string>() { "genre_5", "genre_6" },
                DescriptionText = "artist bio text",
                Title = "New artist name",
                CoverImageId = _validImageId,
                PublishedStatus = PublishStatus.UNPUBLISHED,
                ArtistId = _validArtistId,
                ArtistName = "artist name 2",
                Label = "Label 2",
                Price = 10.45,
                Producer = "Producer 2",
                ReleaseDate = new DateTime(1998, 2, 2),
                TotalDurationInSeconds = 22,
                Tracks = new List<AlbumTrack>()
            };
            var updateResult = await repo.UpdateAsync(existingAlbumId, updateModel);

            Assert.Equal(updateModel.Title, updateResult.Title);
            Assert.Equal(updateModel.PublishedStatus, updateResult.PublishedStatus);
            Assert.Equal(updateModel.DescriptionText, updateResult.DescriptionText);
            Assert.Equal(updateModel.Label, updateResult.Label);
            Assert.Equal(updateModel.Price, updateResult.Price);
            Assert.Equal(updateModel.Producer, updateResult.Producer);
            Assert.Equal(updateModel.ReleaseDate, updateResult.ReleaseDate);
            Assert.Equal(updateModel.TotalDurationInSeconds, updateResult.TotalDurationInSeconds);
            Assert.Equal(updateModel.Genres, updateResult.Genres);
            Assert.Equal(updateModel.Tracks, updateResult.Tracks);
        }

        [Fact]
        public async void UpdateAlbum_UpdatesTracksWhenTrackIdsAreSuppliedForTracks()
        {
            int existingAlbumId = await SetupValidAlbumRecordInDatabase();
            Album a = await this.repo.GetAsync(existingAlbumId);

            int[] originalTrackIds = a.Tracks.Select(t => t.TrackId.Value).ToArray();

            foreach(var t in a.Tracks)
            {
                t.Title = "new_title";
            }
            AlbumDetail result = await repo.UpdateAsync(existingAlbumId, a);

            foreach(int trackId in originalTrackIds)
            {
                Assert.Contains(result.Tracks, x => x.TrackId == trackId);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(12)]
        [InlineData(15)]
        public async void UpdateAlbum_ReturnsTheCorrectNumberOfTracks_WhenOverWritingTracksWithoutValidTrackIds(int numTracks)
        {
            int existingAlbumId = await SetupValidAlbumRecordInDatabase();
            Album a = await this.repo.GetAsync(existingAlbumId);

            List<AlbumTrack> newTracks = new List<AlbumTrack>();
            for (int i = 0; i < numTracks; i++)
            {
                newTracks.Add(new AlbumTrack
                {
                    DurationInSeconds = 100,
                    Title = "new_title",
                    TrackId = null, // null id should replace record
                    TrackNumber = i
                });
            }
            a.Tracks = newTracks;
            AlbumDetail result = await repo.UpdateAsync(existingAlbumId, a);

            Assert.Equal(newTracks.Count, result.Tracks.Count);
            
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(12)]
        [InlineData(15)]
        public async void UpdateAlbum_OverWritesTracksWithNewRecords_WhenNoTrackIdsAreSuppliedForTracks(int numTracks)
        {
            int existingAlbumId = await SetupValidAlbumRecordInDatabase();
            Album a = await this.repo.GetAsync(existingAlbumId);

            int[] originalTrackIds = a.Tracks.Select(t => t.TrackId.Value).ToArray();

            List<AlbumTrack> newTracks = new List<AlbumTrack>();
            for (int i = 0; i < numTracks; i++)
            {
                newTracks.Add(new AlbumTrack
                {
                    DurationInSeconds = 100,
                    Title = "new_title",
                    TrackId = null,
                    TrackNumber = i
                });
            }
            a.Tracks = newTracks;
            AlbumDetail result = await repo.UpdateAsync(existingAlbumId, a);
            
            // test that all prev ids were removed
            foreach (int trackId in originalTrackIds)
            {
                Assert.DoesNotContain(result.Tracks, x => x.TrackId == trackId);
            }
        }
    }
}