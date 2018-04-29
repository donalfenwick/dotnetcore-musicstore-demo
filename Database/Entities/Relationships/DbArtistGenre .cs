using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities.Relationships
{
    public class DbArtistGenre
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; }

        public int ArtistId { get; set; }
        public virtual DbArtist Artist { get; set; }

        public int GenreId { get; set; }
        public virtual DbGenre Genre { get; set; }
    }
}
