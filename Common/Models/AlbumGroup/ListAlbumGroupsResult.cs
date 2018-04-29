using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.AlbumGroup
{
    [DataContract(Name = "ListAlbumGroupsResult")]
    public class ListAlbumGroupsResult
    {
        [DataMember]
        public ICollection<AlbumGroupDetail> Items { get; set; } = new List<AlbumGroupDetail>();
    }
}
