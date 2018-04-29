using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.GenreViewModels
{
    public class GenreIndexPageViewModel
    {
        public List<GenreItemViewModel> Genres { get; set; } = new List<GenreItemViewModel>();
    }
}
