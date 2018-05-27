using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MusicStoreDemo.Common.Models
{
    [DataContract(Name="ApiError")]
    public class ApiErrorRep
    {
        public ApiErrorRep(string message)
        {
            this.Message = message;
        }
        [DataMember]
        public string Message { get; set; }
    }
}
