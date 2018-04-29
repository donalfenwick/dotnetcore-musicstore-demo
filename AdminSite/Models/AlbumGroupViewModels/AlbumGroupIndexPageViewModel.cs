using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels
{
    public class AlbumGroupIndexPageViewModel
    {
        public List<AlbumGroupViewModel> Items { get; set; } = new List<AlbumGroupViewModel>();
    }
}
