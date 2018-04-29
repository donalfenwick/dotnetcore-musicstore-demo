using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.AlbumGroup
{
    [DataContract(Name = "AlbumGroupItemPosition")]
    public class AlbumGroupItemPosition
    {
        [DataMember]
        public int AlbumId { get; set; }
        [DataMember]
        public string AlbumTitle { get; set; }
        [DataMember]
        public int PositionIndex { get; set; }
    }
}
