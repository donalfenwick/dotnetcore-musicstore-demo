using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities
{
    [Table("Track")]
    public class DbTrack
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
        public int DurationInSeconds { get; set; }


        public int AlbumId { get; set; }
        public virtual DbAlbum Album { get; set; }
    }
}
