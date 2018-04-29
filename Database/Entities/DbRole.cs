using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities
{
    [Table("Role")]
    public class DbRole : IdentityRole<string>
    {
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
