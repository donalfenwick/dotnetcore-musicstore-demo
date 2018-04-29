using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Album
{
    [DataContract(Name="Albumist")]
    public class AlbumList : IPaginatedList<AlbumDetail>
    {
        [DataMember]
        public ICollection<AlbumDetail> Items {get; set;} = new List<AlbumDetail>();
        [DataMember]
        public int TotalItems { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
    }
}