using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.GenreViewModels
{
    public class GenreItemViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int TotalArtists { get; set; }
        public int TotalAlbums { get; set; }
    }
}
