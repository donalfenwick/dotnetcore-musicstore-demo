using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels
{
    public class AlbumGroupViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public int TotalAlbums { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
