using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.Common.Models.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.Mappers
{
    public class GenreViewModelMapper
    {
        public GenreItemViewModel MapToViewModel(GenreDetail sourceObject)
        {
            return new GenreItemViewModel
            {
                Name = sourceObject.Name,
                Created = sourceObject.Created,
                TotalAlbums = sourceObject.TotalAlbums,
                TotalArtists = sourceObject.TotalArtists
            };
        }

        public SelectableGenreViewModel Map(GenreDetail sourceObject)
        {
            return new SelectableGenreViewModel
            {
                Name = sourceObject.Name,
                IsSelected = false
            };
        }
    }
}
