using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.ArtistViewModels
{
    public class ArtistListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public PublishStatus? Status { get; set; }
    }
}
