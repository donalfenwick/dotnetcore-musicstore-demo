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
using MusicStoreDemo.Common.Models.AlbumGroup;

namespace MusicStoreDemo.Tests.Api.Repositories
{
    [Trait("Category", "RepositoryTests")]
    public class AlbumGroupRepositoryTests : IDisposable
    {
        private AlbumGroupRepository repo;
        private readonly MusicStoreDbContext db;
        private const string VALID_GROUP_KEY = "TEST_KEY";


        public AlbumGroupRepositoryTests()
        {
            // set up test data
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db" + Guid.NewGuid().ToString())
                .Options;
            this.db = new MusicStoreDbContext(options);


            var loggerMock = new Mock<ILogger<ArtistController>>();
            
            this.repo = new AlbumGroupRepository(this.db, new AlbumGroupMapper());
            
        }

        public void Dispose()
        {
            // tear down in memory db after each test
            this.db.Database.EnsureDeleted();
            this.db.Dispose();
        }

        // returns a valid object model that should succeed when calling the api
        private AlbumGroup CreateValidCreateModel()
        {
            return new AlbumGroup()
            {
                Key = VALID_GROUP_KEY,
                Name = "test group"
            };
        }


        // sets up a valid record in the database for get/update tests
        private async Task<int> SetupValidRecordInDatabase()
        {
            // create code is tested in other unit tests
            var result = await this.repo.AddAsync(CreateValidCreateModel());
            return result.Id;
        }

        private async Task<int> SetupTestAlbumInDatabase()
        {
            var testAlbum = new DbAlbum
            {
                Title = "test_album",
                CreatedUtc = DateTime.UtcNow,
                PublishStatus = DbPublishedStatus.PUBLISHED,
                ReleaseDate = DateTime.Now,
                UpdatedUtc = DateTime.UtcNow,
                Artist = new DbArtist
                {
                    Name = "test artist",
                    PublishStatus = DbPublishedStatus.PUBLISHED,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                }
            };
            this.db.Albums.Add(testAlbum);
            await this.db.SaveChangesAsync();
            return testAlbum.Id;
        }

        [Fact]
        public async void AddAlbumGroup_ReturnsNonNullResult_WithValidInput(){
            AlbumGroup input = CreateValidCreateModel();
            AlbumGroupDetail result = await this.repo.AddAsync(input);
            Assert.NotNull(result);
        }

        [Fact]
        public async void AddAlbumGroup_ReturnsAlbumGroupDetailResult_WithValidInput()
        {
            AlbumGroup input = CreateValidCreateModel();
            AlbumGroupDetail result = await this.repo.AddAsync(input);
           
            Assert.IsType<AlbumGroupDetail>(result);
        }

        [Fact]
        public async void GetAlbumGroup_ReturnsCorrectResultWithValidId()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroupDetail result = await this.repo.GetAsync(groupId);
            Assert.NotNull(result);
            Assert.IsType<AlbumGroupDetail>(result);
        }

        [Fact]
        public async void GetAlbumGroup_ReturnsCorrectResultWithValidKey()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroupDetail result = await this.repo.GetAsync(VALID_GROUP_KEY);
            Assert.NotNull(result);
            Assert.IsType<AlbumGroupDetail>(result);
        }

        [Fact]
        public async void GetAlbumGroup_ReturnsNullWithInvalidId()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroupDetail result = await this.repo.GetAsync(56256757);
            Assert.Null(result);
        }

        [Fact]
        public async void GetAlbumGroup_ReturnsNullWithInvalidKey()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroupDetail result = await this.repo.GetAsync("non_valid_key_input_string");
            Assert.Null(result);
        }

        [Fact]
        public async void AddAlbumGroup_ThrowsEntityAlreadyExistsRepositoryException_WhenAttemptingToCreateAnExistingGroupKey()
        {
            AlbumGroup input = CreateValidCreateModel();
            AlbumGroupDetail result = await this.repo.AddAsync(input);

            await Assert.ThrowsAsync<EntityAlreadyExistsRepositoryException>(() => this.repo.AddAsync(input));
        }

        [Fact]
        public async void ListAlbumGroup_ReturnsNotNull()
        {
            int groupId = await SetupValidRecordInDatabase();
            Assert.NotNull(await this.repo.ListAsync());
        }

        [Fact]
        public async void ListAlbumGroup_ReturnsCollectionOfAlbumGroupDetail()
        {
            int groupId = await SetupValidRecordInDatabase();
            
            ICollection<AlbumGroupDetail> result = await this.repo.ListAsync();
            Assert.IsAssignableFrom<ICollection<AlbumGroupDetail>>(result);
        }


        [Fact]
        public async void UpdateAlbumGroup_ReturnsAlbumGroupDetailResult_WithValidInput()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroup input = CreateValidCreateModel();
            input.Name = "NewName";
            AlbumGroupDetail result = await this.repo.UpdateAsync(groupId, input);
            Assert.IsType<AlbumGroupDetail>(result);
        }

        [Fact]
        public async void UpdateAlbumGroup_ReturnsMatchingName_WithValidInput()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroup input = CreateValidCreateModel();
            input.Name = "NewName";
            AlbumGroupDetail result = await this.repo.UpdateAsync(groupId, input);
            Assert.Equal("NewName",result.Name);
        }

        [Fact]
        public async void UpdateAlbumGroup_ThrowsEntityNotFoundRepositoryException_WithInvalidGroupId()
        {
            int groupId = await SetupValidRecordInDatabase();
            AlbumGroup input = CreateValidCreateModel();
            input.Name = "NewName";
            await Assert.ThrowsAsync<EntityNotFoundRepositoryException>( ()=> this.repo.UpdateAsync(99999, input));

        }

        [Fact]
        public async void AddAlbumToAlbumGroup_GroupContainsAlbumAfterCall()
        {
            int groupId = await SetupValidRecordInDatabase();
            int albumId = await SetupTestAlbumInDatabase();
            
            await this.repo.AddAlbumAsync(groupId, albumId);

            Assert.NotNull(await this.db.AlbumGroupListPositions.SingleOrDefaultAsync(x => x.GroupId == groupId && x.AlbumId == albumId));
        }

        [Fact]
        public async void AddAlbumToAlbumGroup_AlbumIsOnlyAddedOnceIfAddedMultipleTimes()
        {
            int groupId = await SetupValidRecordInDatabase();
            int albumId = await SetupTestAlbumInDatabase();

            await this.repo.AddAlbumAsync(groupId, albumId);

            Assert.Equal(1, await this.db.AlbumGroupListPositions.CountAsync(x => x.GroupId == groupId && x.AlbumId == albumId));
        }

        [Fact]
        public async void AddAlbumToAlbumGroup_WithInvalidGroupId_ThrowsEntityNotFoundRepositoryException()
        {
            int groupId = 12123;
            int albumId = await SetupTestAlbumInDatabase();

            await Assert.ThrowsAsync<EntityNotFoundRepositoryException>(() =>  this.repo.AddAlbumAsync(groupId, albumId));
        }

        [Fact]
        public async void AddAlbumToAlbumGroup_WithInvalidAlbumId_ThrowsEntityNotFoundRepositoryException()
        {
            int groupId = await SetupValidRecordInDatabase();
            int albumId = 3435;

            await Assert.ThrowsAsync<EntityNotFoundRepositoryException>(() => this.repo.AddAlbumAsync(groupId, albumId));
        }

        [Fact]
        public async void RemoveAlbumFromAlbumGroup_AlbumNotContainedInGroupAfterCall()
        {
            int groupId = await SetupValidRecordInDatabase();
            int albumId = await SetupTestAlbumInDatabase();

            await this.repo.AddAlbumAsync(groupId, albumId);
            await this.repo.RemoveAlbumAsync(groupId, albumId);
            Assert.Equal(0, await this.db.AlbumGroupListPositions.CountAsync(x => x.GroupId == groupId && x.AlbumId == albumId));
        }

        [Fact]
        public async void RemoveAlbumFromAlbumGroup_CanCallRemoveAlbumMoreThanOnce()
        {
            int groupId = await SetupValidRecordInDatabase();
            int albumId = await SetupTestAlbumInDatabase();

            await this.repo.AddAlbumAsync(groupId, albumId);
            await this.repo.RemoveAlbumAsync(groupId, albumId);
            await this.repo.RemoveAlbumAsync(groupId, albumId);
            Assert.Equal(0, await this.db.AlbumGroupListPositions.CountAsync(x => x.GroupId == groupId && x.AlbumId == albumId));
        }



    }
}