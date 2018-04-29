using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Artist
{
    [DataContract(Name = "Artist")]
    public class Artist 
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(200)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [MaxLength(2000)]
        public string BioText { get; set; }

        [DataMember]
        public ICollection<string> Genres { get; set; }

        [DataMember]
        public int? BioImageId { get; set; }

        [IgnoreDataMember]
        public PublishStatus PublishedStatus { get; set; }
    }
}
