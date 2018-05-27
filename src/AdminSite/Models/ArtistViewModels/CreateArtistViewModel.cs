using Microsoft.AspNetCore.Http;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.ArtistViewModels
{
    public class CreateArtistViewModel
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string BioText { get; set; }

        public List<SelectableGenreViewModel> Genres { get; set; } = new List<SelectableGenreViewModel>();

        public IFormFile BioImage { get; set; }
    }
}
