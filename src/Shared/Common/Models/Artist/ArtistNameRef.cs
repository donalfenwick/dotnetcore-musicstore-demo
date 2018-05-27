using System;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Common.Models.Artist
{
    public class ArtistNameRef
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PublishStatus Status { get; set; }
    }
}
