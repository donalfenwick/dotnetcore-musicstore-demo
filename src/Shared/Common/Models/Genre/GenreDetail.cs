using System;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Genre
{
    [DataContract(Name="GenreDetail")]
    public class GenreDetail: Genre
    {
        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public int TotalAlbums { get; set; }

        [DataMember]
        public int TotalArtists { get; set; }
    }
}
