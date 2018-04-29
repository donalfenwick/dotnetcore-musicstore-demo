using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.Genre
{
    [DataContract(Name = "Genre")]
    public class Genre
    {
        [DataMember]
        public string Name { get; set; }
    }
}
