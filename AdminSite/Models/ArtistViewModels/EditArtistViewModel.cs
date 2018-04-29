using MusicStoreDemo.Common.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.ArtistViewModels
{
    public class EditArtistViewModel : CreateArtistViewModel
    {
        public int Id { get; set; }
        public int? BioImageId { get; set; }
        public PublishStatus PublishedStatus { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
