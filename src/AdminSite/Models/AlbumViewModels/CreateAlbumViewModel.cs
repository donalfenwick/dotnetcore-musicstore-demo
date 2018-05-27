using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.Common.Models.Artist;

namespace MusicStoreDemo.AdminSite.Models.AlbumViewModels
{
    public class CreateAlbumViewModel
    {
        [Required]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string DescriptionText { get; set; }


        [MaxLength(200)]
        public string Producer { get; set; }
        [MaxLength(200)]
        public string Label { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int ArtistId { get; set; }
        public List<SelectableGenreViewModel> Genres { get; set; } = new List<SelectableGenreViewModel>();

		public List<CheckBoxListItem> Groups { get; set; } = new List<CheckBoxListItem>();

        public IFormFile CoverImage { get; set; }

        public List<SelectListItem> ArtistOptions { get; set; } = new List<SelectListItem>();

        public List<AlbumTrackViewModel> Tracks { get; set; } = new List<AlbumTrackViewModel>();
    }
}
