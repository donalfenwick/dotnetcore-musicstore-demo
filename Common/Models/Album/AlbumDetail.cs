using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Album
{
    [DataContract(Name="AlbumDetail")]
    public class AlbumDetail : Album
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CreatedUtc { get; set; }

        [DataMember]
        public DateTime UpdatedUtc { get; set; }
        
        [DataMember]
        public string CoverImageUrl { get; set; }
    }
}
