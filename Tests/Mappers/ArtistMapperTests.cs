using Moq;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Collections;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Tests.Mappers
{
    [Trait("Category", "MapperTests")]
    public class ArtistMapperTests
    {
        private readonly DbArtist _validDbArtist;

        public ArtistMapperTests()
        {
            ICollection<DbArtistGenre> artistGenres = (new List<string> { "genre1", "genre2", "genre3" }).Select( x => 
                    new DbArtistGenre { Genre = new DbGenre() { Name = x, CreatedUtc = DateTime.UtcNow, UpdatedUtc = DateTime.UtcNow } }).ToList();

            DbArtist artist = new DbArtist()
            {
                Id = 44,
                Name = "SampleName",
                BioText = "SampleText",
                CreatedUtc = DateTime.UtcNow,
                UpdatedUtc = DateTime.UtcNow,
                PublishStatus = DbPublishedStatus.PUBLISHED,
            };

            // artist genres is a virtual readonly property mock this property
            // and manualy set the rest of the values on the resulting object
            var stub = new Mock<DbArtist>();
            stub.SetupGet(x => x.ArtistGenres).Returns(artistGenres);
            
            stub.Object.Id = 44;
            stub.Object.Name = "SampleName";
            stub.Object.BioText = "SampleText";
            stub.Object.CreatedUtc = DateTime.UtcNow;
            stub.Object.UpdatedUtc = DateTime.UtcNow;
            stub.Object.PublishStatus = DbPublishedStatus.PUBLISHED;
            _validDbArtist = stub.Object;
        }

        [Fact]
        public void MapToDetailRep_DoesNotReturnNull_WithValidInput()
        {
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);

            Assert.NotNull(result);
        }

        [Fact]
        public void MapToDetailRep_ThrowsNullReferenceException_WithNullInput()
        {
            ArtistMapper mapper = new ArtistMapper();
            Assert.Throws<NullReferenceException>(() => mapper.MapToDetailRep(null));
        }

        [Fact]
        public void MapToDetailRep_HasExpectedId_WithValidInput()
        {
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Equal(_validDbArtist.Id, result.Id);
        }

        [Fact]
        public void MapToDetailRep_HasExpectedName_WithValidInput()
        {
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Equal(_validDbArtist.Name, result.Name);
        }

        [Fact]
        public void MapToDetailRep_HasExpectedBioText_WithValidInput()
        {
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Equal(_validDbArtist.BioText, result.BioText);
        }

        [Theory]
        [InlineData(DbPublishedStatus.ARCHIVED)]
        [InlineData(DbPublishedStatus.DELETED)]
        [InlineData(DbPublishedStatus.PUBLISHED)]
        [InlineData(DbPublishedStatus.UNPUBLISHED)]
        public void MapToDetailRep_HasPublishedStatus_WithValidInput(DbPublishedStatus publishedStatusTestCase)
        {
            DbArtist sourceObj = _validDbArtist;
            sourceObj.PublishStatus = publishedStatusTestCase;
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            ArtistMapper mapper = new ArtistMapper();
            PublishStatus expetedStatus = statusMapper.Map(publishedStatusTestCase);
            ArtistDetail result = mapper.MapToDetailRep(sourceObj);
            ICollection<string> dbGenres = _validDbArtist.ArtistGenres.Select(x => x.Genre.Name).ToList();
            Assert.Equal(expetedStatus, result.PublishedStatus);
        }

        [Fact]
        public void MapToDetailRep_HasExpectedBioImageId_WithValidInput()
        {
            DbArtist sourceObj = _validDbArtist;
            sourceObj.BioImageId = 1515;
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Equal(sourceObj.BioImageId, result.BioImageId);
        }

        [Fact]
        public void MapToDetailRep_HasNullBioImageId_WithNullInput()
        {
            DbArtist sourceObj = _validDbArtist;
            sourceObj.BioImageId = null;
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Null(result.BioImageId);
        }

        [Fact]
        public void MapToDetailRep_HasExpectedBioImageUrl_WithValidInput()
        {
            DbArtist sourceObj = _validDbArtist;
            sourceObj.BioImageId = 1515;
            ArtistMapper mapper = new ArtistMapper();
            ArtistDetail result = mapper.MapToDetailRep(_validDbArtist);
            Assert.Equal("/api/image/1515", result.BioImageUrl);
        }
    }
}
