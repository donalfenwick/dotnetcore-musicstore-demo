using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MusicStoreDemo.Database.Entities.Relationships;

namespace MusicStoreDemo.Database.Entities
{
    [Table("Artist")]
    public class DbArtist
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string BioText { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public DbPublishedStatus PublishStatus { get; set; }

        public int? BioImageId { get; set; }
        public virtual DbImageResource BioImage { get; set; }
        public virtual ICollection<DbAlbum> Albums { get; set; }
        public virtual ICollection<DbArtistGenre> ArtistGenres { get; } = new List<DbArtistGenre>();
    }
}
