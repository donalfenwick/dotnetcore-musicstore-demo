using MusicStoreDemo.Database.Entities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities
{
    [Table("Genre")]
    public class DbGenre
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        
        public virtual ICollection<DbAlbumGenre> AlbumGenres { get; } = new List<DbAlbumGenre>();
        public virtual ICollection<DbArtistGenre> ArtistGenres { get; } = new List<DbArtistGenre>();
    }
}
