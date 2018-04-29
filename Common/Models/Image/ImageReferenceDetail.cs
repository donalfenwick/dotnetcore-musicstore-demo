using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MusicStoreDemo.Common.Models.Image
{
    [DataContract(Name ="ImageRef")]
    public class ImageReferenceDetail
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string MimeType { get; set; }
        //data is only used internally within the api and is not returned to the client directly
        [IgnoreDataMember]
        public byte[] Data { get; set; }
    }
}
