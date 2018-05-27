using MusicStoreDemo.AdminSite.Models.ArtistViewModels;
using MusicStoreDemo.Common.Models.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreDemo.AdminSite.Models.Mappers
{
    public class ArtistViewModelMapper
    {
        public ArtistListItemViewModel Map(ArtistDetail i)
        {
            return new ArtistListItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Created = i.CreatedUtc,
                Updated = i.UpdatedUtc,
                Status = i.PublishedStatus
            };
        }
    }
}
