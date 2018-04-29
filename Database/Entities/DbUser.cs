using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities
{
    [Table("User")]
    public class DbUser : IdentityUser<string>
    {
        [MaxLength(100)]
        public string Firstname { get; set; }
        [MaxLength(100)]
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
