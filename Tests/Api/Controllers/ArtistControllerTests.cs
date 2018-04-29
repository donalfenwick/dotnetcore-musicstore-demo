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

namespace MusicStoreDemo.Tests.Api.Controllers
{
    [Trait("Category", "ApiControllerTests")]
    public class ArtistControllerTests : IDisposable
    {
        private readonly Mock<ILogger<ArtistController>> _loggerMock;
        private readonly Mock<IArtistRepository> _repoMock;

        private readonly string[] _validGenres = { "genre_1", "genre_2", "genre_3","genre_4", "genre_5", "genre_6" };

        private int _existingArtistId = 1111;

        public ArtistControllerTests()
        {
            _loggerMock = new Mock<ILogger<ArtistController>>();
            _repoMock = new Mock<IArtistRepository>();
        }

        public void Dispose()
        {
        }

        private Mock<IArtistRepository> SetupMockWithValidReturnValuesForArtistInput(Artist artist, int artistId){
            
            Mock<IArtistRepository> mock = new Mock<IArtistRepository>();
            
            ArtistDetail validResult = new ArtistDetail{
                Id = artistId,
                BioText = artist.BioText,
                CreatedUtc = DateTime.UtcNow,
                Genres = artist.Genres,
                Name = artist.Name,
                UpdatedUtc = DateTime.UtcNow,
                PublishedStatus = artist.PublishedStatus
            };
            mock.Setup( m => m.UpdateAsync(_existingArtistId, artist)).ReturnsAsync(validResult);
            mock.Setup( m => m.AddAsync(artist)).ReturnsAsync(validResult);
            mock.Setup( m => m.GetAsync(_existingArtistId)).ReturnsAsync(validResult);

            return mock;
        }

        // returns a valid object model that suhould succeed when calling the api
        private Artist CreateValidCreateModel(){
            return new Artist()
            {
                Name = "test artist",
                Genres = new List<string> { "genre_1" },
                BioText = "Artist text bio",
                BioImageId = 1111,
                PublishedStatus = PublishStatus.UNPUBLISHED
            };
        }

     
        [Fact]
        public async void AddAction_ReturnsCCreatedAtRouteResult_WithValidInput()
        {
            _repoMock.Setup(m => m.AddAsync(It.IsAny<Artist>())).ReturnsAsync(new ArtistDetail());
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Add(input);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async void AddAction_ReturnsCreatedAtRouteResultWithArtistDetailRep_WithValidInput()
        {
            _repoMock.Setup(m => m.AddAsync(It.IsAny<Artist>())).ReturnsAsync(new ArtistDetail());
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Add(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;

            Assert.IsType<ArtistDetail>(createdResult.Value);
        }

        [Fact]
        public async void AddAction_ReturnsCreatedAtRouteResultWithCorrectRouteValues()
        {
            int newId = 1111;
            _repoMock.Setup(m => m.AddAsync(It.IsAny<Artist>())).ReturnsAsync(new ArtistDetail(){ Id = newId });
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Add(input);
            CreatedAtRouteResult createdResult = result as CreatedAtRouteResult;
            
            Assert.Equal("artist", createdResult.RouteValues["controller"].ToString(),ignoreCase: true);
            Assert.Equal(nameof(ArtistController.Get), createdResult.RouteValues["action"].ToString(),ignoreCase: true);
            Assert.Equal(newId.ToString(), createdResult.RouteValues["id"].ToString());
        }

        [Fact]
        public async void AddAction_ReturnsBadRequestOBjectResult_WhenRepoThrowsRepositoryException()
        {
            _repoMock.Setup(m => m.AddAsync(It.IsAny<Artist>())).ThrowsAsync(new RepositoryException("Invalid genre"));
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Add(input);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void UpdateAction_ReturnsNotFoundObjectResult_WhenRepoThrowsEntityNotFoundRepositoryException()
        {
            _repoMock.Setup(m => m.UpdateAsync(It.IsAny<int>(), It.IsAny<Artist>())).ThrowsAsync(new EntityNotFoundRepositoryException("Artist not found"));
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Update(1111, input);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void Updatection_ReturnsBadRequestOBjectResult_WhenRepoThrowsRepositoryException()
        {
            _repoMock.Setup(m => m.UpdateAsync(It.IsAny<int>(), It.IsAny<Artist>())).ThrowsAsync(new RepositoryException("Invalid genre"));
            ArtistController ctrl = new ArtistController(_repoMock.Object, _loggerMock.Object);
            Artist input = CreateValidCreateModel();
            IActionResult result = await ctrl.Update(1111, input);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void AddAction_ReturnsResponseWithMatchingProperties()
        {
            Artist input = CreateValidCreateModel();
            int resultArtistId = 1111;
            var mock = SetupMockWithValidReturnValuesForArtistInput(input, resultArtistId);
            ArtistController ctrl = new ArtistController(mock.Object, _loggerMock.Object);
            
            IActionResult result = await ctrl.Add(input);
            var createdRestul = result as CreatedAtRouteResult;
            var createdObj = createdRestul.Value as ArtistDetail;

            Assert.Equal(input.Name, createdObj.Name);
            Assert.Equal(input.PublishedStatus, createdObj.PublishedStatus);
            Assert.Equal(input.BioText, createdObj.BioText);
            Assert.Equal(input.Genres, createdObj.Genres);
            Assert.Equal(resultArtistId, createdObj.Id);
        }

        [Fact]
        public async void UpdateAction_ReturnsSameObjectPropertiesAsRepository()
        {
            int existingId = 1111;
            Artist artist = CreateValidCreateModel();
            var mock = SetupMockWithValidReturnValuesForArtistInput(artist, existingId);
            ArtistController ctrl = new ArtistController(mock.Object, _loggerMock.Object);
            var updateActionResult = await ctrl.Update(existingId, artist);
            var updateResult = updateActionResult as ObjectResult;
            var updateObj = updateResult.Value as ArtistDetail;

            Assert.Equal(artist.Name, updateObj.Name);
            Assert.Equal(artist.PublishedStatus, updateObj.PublishedStatus);
            Assert.Equal(artist.BioText, updateObj.BioText);
            Assert.Equal(artist.Genres, updateObj.Genres);
            Assert.Equal(existingId, updateObj.Id);
        }

    }
}
