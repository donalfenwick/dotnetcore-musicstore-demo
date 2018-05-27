using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStoreDemo.AdminSite.Models.AlbumViewModels
{
    public class AlbumTrackViewModel
    {
        public int? Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        [Required]
        public int TrackNumber { get; set; }
        public int DurationInSec { get; set; }
    }
}
