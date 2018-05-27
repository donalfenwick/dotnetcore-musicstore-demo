using MusicStoreDemo.Database.Entities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStoreDemo.Database.Entities
{
    [Table("Album")]
    public class DbAlbum
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string DescriptionText { get; set; }
        [MaxLength(200)]
        public string Producer { get; set; }
        [MaxLength(200)]
        public string Label { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public int TotalDurationInSeconds { get; set; }
        public double Price { get; set; } 
        public DateTime ReleaseDate { get; set; }
        public DbPublishedStatus PublishStatus { get; set; }

        public int ArtistId { get; set; }
        public DbArtist Artist { get; set; }
        public int? AlbumCoverImageId { get; set; }
        public virtual DbImageResource AlbumCoverImage { get; set;}
        public virtual ICollection<DbAlbumGenre> AlbumGenres { get; } = new List<DbAlbumGenre>();
        public virtual ICollection<DbTrack> Tracks { get; } = new List<DbTrack>();

        public virtual ICollection<DbAlbumGroupAlbumPosition> GroupPositions { get; } = new List<DbAlbumGroupAlbumPosition>();
        public virtual ICollection<DbUserPurchasedAlbum> UserPurchases { get; } = new List<DbUserPurchasedAlbum>();
    }
}
