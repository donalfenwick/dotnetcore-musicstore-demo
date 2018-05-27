using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities.Relationships
{
    public class DbAlbumGroupAlbumPosition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; }

        /// <summary>
        /// Used to order the list items when representing a list in the UI
        /// </summary>
        public int PositionIndex { get; set; }

        public int AlbumId { get; set; }
        public virtual DbAlbum Album { get; set; }

        public int GroupId { get; set; }
        public virtual DbAlbumGroup Group { get; set; }
    }
}
