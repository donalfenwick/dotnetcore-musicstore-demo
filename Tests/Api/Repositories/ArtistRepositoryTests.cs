using System;
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

namespace MusicStoreDemo.Tests.Api.Repositories
{
    [Trait("Category", "RepositoryTests")]
    public class ArtistRepositoryTests : IDisposable
    {
        private ArtistRepository repo;
        private readonly MusicStoreDbContext db;
        private readonly string[] _validGenres = { "genre_1", "genre_2", "genre_3","genre_4", "genre_5", "genre_6" };
        private int _validImageId;
        
        public ArtistRepositoryTests()
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
            db.ImageResources.Add(imageResource);

            db.SaveChanges();
            _validImageId = imageResource.Id;

            var loggerMock = new Mock<ILogger<ArtistController>>();
            
            this.repo = new ArtistRepository(this.db, new ArtistMapper());
            
        }

        public void Dispose()
        {
            // tear down in memory db after each test
            this.db.Database.EnsureDeleted();
            this.db.Dispose();
        }

        // returns a valid object model that should succeed when calling the api
        private Artist CreateValidCreateModel(){
            return new Artist()
            {
                Name = "test artist",
                Genres = new List<string> { "genre_1" },
                BioText = "Artist text bio",
                BioImageId = _validImageId,
                PublishedStatus = PublishStatus.UNPUBLISHED
            };
        }

        // sets up a valid artist in the database for get/update tests
        private async Task<int> SetupValidArtistRecordInDatabase(){
            // create code is tested in other unit tests
            var result = await this.repo.AddAsync(CreateValidCreateModel());
            return result.Id;
        }

        [Fact]
        public async void AddArtist_ReturnsNonNullResult_WithValidInput(){
            Artist input = CreateValidCreateModel();
            ArtistDetail result = await this.repo.AddAsync(input);
            Assert.NotNull(result);
        }

        [Fact]
        public async void AddArtist_ReturnsArtistDetailResult_WithValidInput()
        {
            Artist input = CreateValidCreateModel();
            ArtistDetail result = await this.repo.AddAsync(input);
           
            Assert.IsType<ArtistDetail>(result);
        }

        [Fact]
        public async void AddArtist_ThrowsRepoException_WhenInvalidGenreIsSupplied()
        {
            Artist input = CreateValidCreateModel();
            // chenge input to have invalid genres
            input.Genres = new List<string> { "invalidGenreName_534546" };
            await Assert.ThrowsAsync<RepositoryException>( () => this.repo.AddAsync(input) );
        }

        [Fact]
        public async void AddArtist_ReturnsResultWithMatchingProperties()
        {
            Artist input = CreateValidCreateModel();
            ArtistDetail createdObj = await this.repo.AddAsync(input);

            Assert.Equal(input.Name, createdObj.Name);
            Assert.Equal(input.PublishedStatus, createdObj.PublishedStatus);
            Assert.Equal(input.BioText, createdObj.BioText);
            Assert.Equal(input.Genres, createdObj.Genres);
        }

        [Fact]
        public async void GetArtist_ReturnsNullWhenNoArtistExists()
        {
            ArtistDetail result = await this.repo.GetAsync(929292);
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateArtist_ReturnsResultWithMatchingProperties()
        {
            int existingArtistId = await SetupValidArtistRecordInDatabase();
            Artist updateModel = new Artist
            {
                Genres = new List<string>() { "genre_5", "genre_6" },
                BioText = "artist bio text",
                Name = "New artist name",
                BioImageId = _validImageId
            };
            var updateResult = await repo.UpdateAsync(existingArtistId, updateModel);
            
            Assert.Equal(updateModel.Name, updateResult.Name);
            Assert.Equal(updateModel.PublishedStatus, updateResult.PublishedStatus);
            Assert.Equal(updateModel.BioText, updateResult.BioText);
            Assert.Equal(updateModel.Genres, updateResult.Genres);
        }
    }
}