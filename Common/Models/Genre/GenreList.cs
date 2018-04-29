using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models.Genre
{
    [DataContract(Name="GenreList")]
    public class GenreList
    {
        [DataMember]
        public ICollection<GenreDetail> Genres { get; set; }
    }
}
