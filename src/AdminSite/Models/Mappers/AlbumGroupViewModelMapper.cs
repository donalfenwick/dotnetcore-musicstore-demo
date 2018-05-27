using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels;
using MusicStoreDemo.Common.Models.AlbumGroup;

namespace MusicStoreDemo.AdminSite.Models.Mappers
{
    public class AlbumGroupViewModelMapper
    {
        public AlbumGroupViewModel Map(AlbumGroupDetail a){
            return new AlbumGroupViewModel
            {
                Id = a.Id,
                Created = a.Created,
                Updated = a.Updated,
                Key = a.Key,
                Name = a.Name,
                TotalAlbums = a.TotalAlbums
            };
        }
        public AlbumGroupDetail Map(AlbumGroupViewModel a)
        {
            return new AlbumGroupDetail
            {
                Id = a.Id,
                Created = a.Created,
                Updated = a.Updated,
                Key = a.Key,
                Name = a.Name,
                TotalAlbums = a.TotalAlbums
            };
        }
    }
}
