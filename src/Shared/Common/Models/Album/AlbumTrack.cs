using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Album
{
    [DataContract(Name="Track")]
    public class AlbumTrack
    {
        public int? TrackId { get; set; }

        [DataMember]
        public int TrackNumber { get; set; }
        [DataMember]
        [MaxLength(200)]
        public string Title { get; set; }
        [DataMember]
        public int DurationInSeconds { get; set; }
    }
}
