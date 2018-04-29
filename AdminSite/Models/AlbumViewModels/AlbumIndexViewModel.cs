using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MusicStoreDemo.AdminSite.Models.AlbumViewModels
{
    public class AlbumIndexViewModel
    {
        public List<AlbumListItemViewModel> Items { get; set; } = new List<AlbumListItemViewModel>();

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
