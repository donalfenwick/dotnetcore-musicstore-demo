using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicStoreDemo.Common.Mappers
{
    public class PublishedStatusEnumMapper
    {
        public DbPublishedStatus Map(PublishStatus input)
        {
            return Enum.Parse<DbPublishedStatus>(input.ToString());
        }

        public PublishStatus Map(DbPublishedStatus input)
        {
            return Enum.Parse<PublishStatus>(input.ToString());
        }

        public PublishStatus GetAllFlags()
        {
            return Enum.Parse<PublishStatus>(string.Join(",", Enum.GetNames(typeof(PublishStatus))));
        }

        public DbPublishedStatus GetAllDbFlags()
        {
            return Enum.Parse<DbPublishedStatus>(string.Join(",", Enum.GetNames(typeof(DbPublishedStatus))));
        }
    }
}
