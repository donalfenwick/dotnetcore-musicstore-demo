using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicStoreDemo.Tests.Mappers
{
    [Trait("Category", "MapperTests")]
    public class PublishedStatusEnumTests
    {

        [Theory]
        [InlineData(PublishStatus.ARCHIVED, DbPublishedStatus.ARCHIVED)]
        [InlineData(PublishStatus.DELETED, DbPublishedStatus.DELETED)]
        [InlineData(PublishStatus.PUBLISHED, DbPublishedStatus.PUBLISHED)]
        [InlineData(PublishStatus.UNPUBLISHED, DbPublishedStatus.UNPUBLISHED)]
        [InlineData(PublishStatus.ARCHIVED | PublishStatus.DELETED, DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED)]
        [InlineData(PublishStatus.ARCHIVED | PublishStatus.DELETED | PublishStatus.PUBLISHED, DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED | DbPublishedStatus.PUBLISHED)]
        [InlineData(PublishStatus.ARCHIVED | PublishStatus.DELETED | PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED, DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED | DbPublishedStatus.PUBLISHED | DbPublishedStatus.UNPUBLISHED)]
        public void MappingToDbEnum_ProducesSameFlagsOnOutput(PublishStatus input, DbPublishedStatus expectedOutput)
        {
            PublishedStatusEnumMapper mapper = new PublishedStatusEnumMapper();
            Assert.Equal(expectedOutput, mapper.Map(input));
        }

        [Theory]
        [InlineData(DbPublishedStatus.ARCHIVED, PublishStatus.ARCHIVED)]
        [InlineData(DbPublishedStatus.DELETED, PublishStatus.DELETED)]
        [InlineData(DbPublishedStatus.PUBLISHED, PublishStatus.PUBLISHED)]
        [InlineData(DbPublishedStatus.UNPUBLISHED, PublishStatus.UNPUBLISHED)]
        [InlineData(DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED, PublishStatus.ARCHIVED | PublishStatus.DELETED)]
        [InlineData(DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED | DbPublishedStatus.PUBLISHED, PublishStatus.ARCHIVED | PublishStatus.DELETED | PublishStatus.PUBLISHED)]
        [InlineData(DbPublishedStatus.ARCHIVED | DbPublishedStatus.DELETED | DbPublishedStatus.PUBLISHED | DbPublishedStatus.UNPUBLISHED, PublishStatus.ARCHIVED | PublishStatus.DELETED | PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED)]
        public void MappingFromDbEnum_ProducesSameFlagsOnOutput(DbPublishedStatus input, PublishStatus expectedOutput)
        {
            PublishedStatusEnumMapper mapper = new PublishedStatusEnumMapper();
            Assert.Equal(expectedOutput, mapper.Map(input));
        }

        [Fact]
        public void MappingFromDbEnum_WithMultipleFlags_HasExpectedFlags()
        {
            PublishedStatusEnumMapper mapper = new PublishedStatusEnumMapper();
            PublishStatus result = mapper.Map(DbPublishedStatus.ARCHIVED|DbPublishedStatus.PUBLISHED);
            Assert.True(result.HasFlag(PublishStatus.ARCHIVED));
            Assert.True(result.HasFlag(PublishStatus.PUBLISHED));
            Assert.False(result.HasFlag(PublishStatus.UNPUBLISHED));
            Assert.False(result.HasFlag(PublishStatus.DELETED));
        }

        [Fact]
        public void MappingToDbEnum_WithMultipleFlags_HasExpectedFlags()
        {
            PublishedStatusEnumMapper mapper = new PublishedStatusEnumMapper();
            DbPublishedStatus result = mapper.Map(PublishStatus.ARCHIVED | PublishStatus.PUBLISHED);
            Assert.True(result.HasFlag(DbPublishedStatus.ARCHIVED));
            Assert.True(result.HasFlag(DbPublishedStatus.PUBLISHED));
            Assert.False(result.HasFlag(DbPublishedStatus.UNPUBLISHED));
            Assert.False(result.HasFlag(DbPublishedStatus.DELETED));
        }
    }
}
