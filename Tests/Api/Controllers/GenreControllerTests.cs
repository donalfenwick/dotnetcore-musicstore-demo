using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Api.Controllers;
using MusicStoreDemo.Common.Models.Genre;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Repositories;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Tests.Api.Controllers
{
    [Trait("Category", "ApiControllerTests")]
    public class GenreControllerTests : IDisposable
    {
        private readonly GenreController ctrl;
        private readonly Mock<ILogger<GenreController>> _loggerMock;
        private readonly Mock<IGenreRepository> _repoMock;
        public GenreControllerTests()
        {
            _repoMock = new Mock<IGenreRepository>();
            _loggerMock = new Mock<ILogger<GenreController>>();
            ctrl = new GenreController(_repoMock.Object, _loggerMock.Object);
        }

        public void Dispose()
        {
        }

        [Fact]
        public async void PutGenre_ShouldReturnHttpCreatedAtRoute(){
            string genreName = "New genre name";
            _repoMock.Setup( m => m.AddAsync(It.IsAny<string>())).ReturnsAsync(new GenreDetail(){ Name = genreName, Created = DateTime.UtcNow });
            IActionResult r = await ctrl.Put(genreName);
            Assert.IsType<CreatedAtRouteResult>(r);
        }

        [Fact]
        public async void PutGenre_ShouldReturnHttpCreatedAtRouteWithCorrectRouteValues(){
            string genreName = "New genre name";
            _repoMock.Setup( m => m.AddAsync(It.IsAny<string>())).ReturnsAsync(new GenreDetail(){ Name = genreName, Created = DateTime.UtcNow });

            IActionResult r = await ctrl.Put(genreName);
            CreatedAtRouteResult createdResult = r as CreatedAtRouteResult;

            Assert.Equal("genre", createdResult.RouteValues["controller"].ToString(),ignoreCase: true);
            Assert.Equal(nameof(GenreController.Get), createdResult.RouteValues["action"].ToString(),ignoreCase: true);
            Assert.Equal(genreName.ToString(), createdResult.RouteValues["genre"].ToString(), ignoreCase: true);
        }

        [Fact]
        public async void PutGenre_ShouldReturnBadRequestWithNoGenre()
        {
            IActionResult r = await ctrl.Put();
            Assert.IsType<BadRequestObjectResult>(r);
        }

        [Fact]
        public async void PutGenre_ShouldReturnBadRequestWithNullGenre()
        {
            IActionResult r = await ctrl.Put(null);
            Assert.IsType<BadRequestObjectResult>(r);
        }

        [Fact]
        public async void PutGenre_ShouldReturnTheSameGenreNameAsReturnedFromTheRepository()
        {
            string genreName = "New genre name";
            _repoMock.Setup( m => m.GetAsync(It.IsAny<string>(), It.IsAny<PublishStatus>())).ReturnsAsync(new GenreDetail(){ Name = genreName, Created = DateTime.UtcNow });
            _repoMock.Setup( m => m.AddAsync(It.IsAny<string>())).ReturnsAsync(new GenreDetail(){ Name = genreName, Created = DateTime.UtcNow });
            IActionResult r = await ctrl.Put(genreName);
            Assert.IsType<OkObjectResult>(r);
        }
    }
}
