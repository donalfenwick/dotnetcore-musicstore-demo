using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Common.Repositories{
    public interface IArtistRepository{
        Task<ArtistDetail> GetAsync(int artistId);
        Task<ArtistList> ListAsync(int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<ICollection<ArtistNameRef>> ListNamesAsync(PublishStatus statusFlags);
        Task<ArtistDetail> AddAsync(Artist artist);
        Task<ArtistDetail> UpdateAsync(int artistId, Artist artist);
    }
}