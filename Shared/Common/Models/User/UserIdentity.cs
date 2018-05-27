using System;
using System.Collections.Generic;

namespace MusicStoreDemo.Common.Models.User
{
    public class UserIdentity
    {
        public string UserId { get;set;}
        public string UserName { get; set; }
        public List<string> DbRoles { get; set; } = new List<string>();
        public List<string> RolesInClaims { get; set; } = new List<string>();
        public bool FoundDbUser { get; set; }
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
