using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels
{
    public class ReorderAlbumGroupItemViewModel
    {
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public int PositionIndex { get; set; }
    }
}
