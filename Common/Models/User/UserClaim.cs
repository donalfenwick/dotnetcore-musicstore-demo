using System;
namespace MusicStoreDemo.Common.Models.User
{
    public class UserClaim
    {
        public UserClaim()
        {
        }

        public string Type { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
    }
}
