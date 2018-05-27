using MusicStoreDemo.Common.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Album
{
    [DataContract(Name="Album")]
    public class Album
    {
        [MaxLength(200)]
        [DataMember]
        public string Title { get; set; }
        [MaxLength(2000)]        
        [DataMember]
        public string DescriptionText { get; set; }
        [MaxLength(200)]
        [DataMember]
        public string Producer { get; set; }
        [MaxLength(200)]
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public int TotalDurationInSeconds { get; set; }
        [DataMember]
        public double Price { get; set; } 
        [DataMember]
        public DateTime ReleaseDate { get; set; }
        [DataMember]
        public ICollection<AlbumTrack> Tracks { get; set; } = new List<AlbumTrack>();
        [DataMember]
        public ICollection<string> Genres { get; set; }
        [DataMember]
        public int? CoverImageId { get; set; }
        [DataMember]
        public int ArtistId { get; set; }
        [DataMember]
        public string ArtistName { get; set; }

        [IgnoreDataMember]
        public PublishStatus PublishedStatus { get; set; }
    }
}
