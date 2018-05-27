using System;
namespace MusicStoreDemo.Database.Entities
{
    [Flags]
    public enum DbPublishedStatus {
        UNPUBLISHED = 1,
        PUBLISHED = 2,
        ARCHIVED = 4,
        DELETED = 8
    }
}
