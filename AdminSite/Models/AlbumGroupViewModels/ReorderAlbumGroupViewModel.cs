using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels
{
    public class ReorderAlbumGroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<ReorderAlbumGroupItemViewModel> Items { get; set; } = new List<ReorderAlbumGroupItemViewModel>();
    }
}
