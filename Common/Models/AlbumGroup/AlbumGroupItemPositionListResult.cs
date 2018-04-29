using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.AlbumGroup
{
    [DataContract(Name = "AlbumGroupItemPositionListResult")]
    public class AlbumGroupItemPositionListResult
    {
        [DataMember]
        public ICollection<AlbumGroupItemPosition> Items { get; set; } = new List<AlbumGroupItemPosition>();
    }
}
