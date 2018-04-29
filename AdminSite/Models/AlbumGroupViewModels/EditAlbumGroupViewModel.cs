using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels
{
    public class EditAlbumGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
