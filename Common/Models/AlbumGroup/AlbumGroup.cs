using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.AlbumGroup
{
    [DataContract(Name = "AlbumGroup")]
    public class AlbumGroup
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
