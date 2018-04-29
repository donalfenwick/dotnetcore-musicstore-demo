using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStoreDemo.Database.Entities
{
    public class DbUserPurchasedAlbum
    {
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int AlbumId { get; set; }
        public virtual DbAlbum Album { get; set; }

        public string UserId { get; set; }
        public virtual DbUser User { get; set; }
    }
}