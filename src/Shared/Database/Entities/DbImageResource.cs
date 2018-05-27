using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace MusicStoreDemo.Database.Entities
{
    [Table("ImageResource")]
    public class DbImageResource
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string MimeType { get; set; }
        [MaxLength(200)]
        public string Filename { get; set; }
        [MaxLength(2097152)]
        //[Column(TypeName = "varbinary(2097152)")]
        public byte[] Data { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}