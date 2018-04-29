using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Artist
{
    [DataContract(Name="ArtistList")]
    public class ArtistList : IPaginatedList<ArtistDetail>
    {
        [DataMember]
        public ICollection<ArtistDetail> Items {get; set;} = new List<ArtistDetail>();
        [DataMember]
        public int TotalItems { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
    }
}