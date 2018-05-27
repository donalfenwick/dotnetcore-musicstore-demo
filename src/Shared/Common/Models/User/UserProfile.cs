using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.User
{
    public class UserProfile
    {
        // username and email address are readonly and dont require validation
        public string Username { get; set; }
        public string EmailAddress { get; set; }

        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        
    }
}