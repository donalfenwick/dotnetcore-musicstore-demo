using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.AlbumGroup
{
    [DataContract(Name = "AlbumGroupDetail")]
    public class AlbumGroupDetail : AlbumGroup
    {
        [DataMember]
        public int  TotalAlbums { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public DateTime Updated { get; set; }
    }
}
