using MusicStoreDemo.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.ArtistViewModels
{
    public class ArtistIndexViewModel
    {
        public List<ArtistListItemViewModel> Items { get; set; } = new List<ArtistListItemViewModel>();

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
