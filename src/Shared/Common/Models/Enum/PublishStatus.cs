using System;
using System.Collections.Generic;
using System.Text;

namespace MusicStoreDemo.Common.Models.Enum
{
    [Flags]
    public enum PublishStatus
    {
        UNPUBLISHED = 1,
        PUBLISHED = 2,
        ARCHIVED = 4,
        DELETED = 8
    }
}
