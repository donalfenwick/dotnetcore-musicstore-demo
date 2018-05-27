using System;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.AdminSite.Models.AlbumViewModels
{
    public class AlbumListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public PublishStatus? Status { get; set; }
    }
}
