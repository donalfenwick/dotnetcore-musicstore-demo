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
using MusicStoreDemo.Database.Entities.Relationships;

namespace MusicStoreDemo.Tests.Api.Repositories
{
    [Trait("Category", "RepositoryTests")]
    public class AlbumRepository_ListByAlbumGroupKeyTests : IDisposable
    {
        private const string VALID_GROUP_KEY = "TEST_GROUP_1";

        private AlbumRepository repo;
        private readonly MusicStoreDbContext db;
        private readonly string[] _validGenres = { "genre_1", "genre_2", "genre_3","genre_4", "genre_5", "genre_6" };
        private int _validImageId;
        private int _validArtistId;
        private int _validGroupId;
        private int[] _validAlbumIds;
        public AlbumRepository_ListByAlbumGroupKeyTests()
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

            List<DbAlbum> testAlbums = new List<DbAlbum>();
            for (int i = 0; i < 10; i++)
            {
                testAlbums.Add(new DbAlbum
                {
                    Title = "test_album_"+i,
                    CreatedUtc = DateTime.UtcNow,
                    PublishStatus = DbPublishedStatus.PUBLISHED,
                    ReleaseDate = DateTime.Now,
                    UpdatedUtc = DateTime.UtcNow,
                    Artist = new DbArtist
                    {
                        Name = "test artist "+i,
                        PublishStatus = DbPublishedStatus.PUBLISHED,
                        CreatedUtc = DateTime.UtcNow,
                        UpdatedUtc = DateTime.UtcNow
                    }
                });
            }
            this.db.Albums.AddRange(testAlbums);

            var group = new DbAlbumGroup
            {
                CreatedUtc = DateTime.UtcNow,
                Key = VALID_GROUP_KEY,
                Name = "test group",
                UpdatedUtc = DateTime.UtcNow
            };
            db.AlbumGroups.Add(group);
            db.SaveChanges();
            _validImageId = imageResource.Id;
            _validArtistId = artist.Id;
            _validGroupId = group.Id;
            _validAlbumIds = testAlbums.Select(x => x.Id).ToArray();
            for(int i = 0; i < _validAlbumIds.Length; i++)
            {
                // insert all albums into the test group
                db.AlbumGroupListPositions.Add(new DbAlbumGroupAlbumPosition
                {
                    AlbumId = _validAlbumIds[i],
                    GroupId = _validGroupId,
                    CreatedUtc = DateTime.UtcNow,
                    PositionIndex = i
                });
            }
            db.SaveChanges();

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
        private Album CreateValidModel(string title){
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
        private async Task<int> SetupValidAlbumRecordInDatabase(string title){
            // create code is tested in other unit tests
            var result = await this.repo.AddAsync(CreateValidModel(title));
            return result.Id;
        }


        [Fact]
        public async void ListByAlbumGroupKeyAsync_ReturnsNonNullResult_WithValidInput()
        {
            
            var result = await this.repo.ListByAlbumGroupKeyAsync(VALID_GROUP_KEY, 0, 24, PublishStatus.PUBLISHED);
            Assert.NotNull(result);
        }
    }
}