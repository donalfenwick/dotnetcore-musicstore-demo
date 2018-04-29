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
    public class GenreRepositoryTests : IDisposable
    {
        private readonly MusicStoreDbContext db;
        private readonly GenreRepository repo;

        private const string _existingGenreName = "EXISTING_GENRE_NAME";

        public GenreRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db"+Guid.NewGuid().ToString())
                .Options;
            this.db = new MusicStoreDbContext(options);

            this.db.Genres.Add(new DbGenre(){ Name =_existingGenreName, CreatedUtc = DateTime.UtcNow });
            this.db.SaveChanges();


            var loggerMock = new Mock<ILogger<ArtistController>>();
            
            this.repo = new GenreRepository(this.db);
        }
        public void Dispose(){
            // tear down in memory db after each test
            this.db.Database.EnsureDeleted();
            this.db.Dispose();
        }

        

        [Fact]
        public async void CreateGenere_ProducesValidModel(){
            var result = await repo.AddAsync(Guid.NewGuid().ToString());
            Assert.IsType<GenreDetail>(result);
        }

        [Fact]
        public async void CreateGenere_ProducesValidModelWhenGenreExists(){
            string genreName = "genre123";
            
            var result = await repo.AddAsync(genreName);
            Assert.IsType<GenreDetail>(result);
        }

        [Fact]
        public async void GetGenre_ShouldReturnGenre_WhenGenreExists()
        {
            var genre = await repo.GetAsync(_existingGenreName, PublishStatus.PUBLISHED);
            Assert.IsType<GenreDetail>(genre);
        }

        [Fact]
        public async void GetGenre_ShouldReturnNull_WhenGenreExists()
        {
            GenreDetail result = await repo.GetAsync("UNKNOW GENRE", PublishStatus.PUBLISHED);
            Assert.Null(result);
        }

        [Fact]
        public async void GetGenre_ProducesMatchingGenre_AfterAddGenreIsCalled()
        {
            string newGenre = Guid.NewGuid().ToString();
            await repo.AddAsync(newGenre);
            GenreDetail getGenreResult = await repo.GetAsync(newGenre, PublishStatus.PUBLISHED);
            Assert.Equal(newGenre, getGenreResult.Name);
        }
    }
}