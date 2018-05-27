using MusicStoreDemo.Common.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumViewModels
{
    public class EditAlbumViewModel : CreateAlbumViewModel
    {
        public int Id { get; set; }
        public int? CoverImageId { get; set; }
        public PublishStatus PublishedStatus { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
