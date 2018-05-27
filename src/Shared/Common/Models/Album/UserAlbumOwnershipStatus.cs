using System;
using System.Runtime.Serialization;

namespace Common.Models.Album
{
    [DataContract]
    public class UserAlbumOwnershipStatus
    {
        [DataMember]
        public bool IsOwned { get; set; }
        [DataMember]
        public DateTime? PurchaseDate { get; set;}

    }
}