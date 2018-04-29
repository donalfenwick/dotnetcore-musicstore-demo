using System;
using System.Collections.Generic;

namespace MusicStoreDemo.Common.Models.User
{
    public class UserIdentity
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
