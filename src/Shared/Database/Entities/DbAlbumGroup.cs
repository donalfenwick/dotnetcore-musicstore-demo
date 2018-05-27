using MusicStoreDemo.Database.Entities.Relationships;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MusicStoreDemo.Database.Entities
{
    public class DbAlbumGroup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public ICollection<DbAlbumGroupAlbumPosition> Items { get; set; } = new List<DbAlbumGroupAlbumPosition>();
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}
