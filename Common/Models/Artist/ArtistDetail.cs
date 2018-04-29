using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Artist
{
    [DataContract(Name = "ArtistDetail")]
    public class ArtistDetail : Artist
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public DateTime CreatedUtc { get; set; }

        [DataMember]
        public DateTime UpdatedUtc { get; set; }

        [DataMember]
        public string BioImageUrl { get; set; }

    }
}
