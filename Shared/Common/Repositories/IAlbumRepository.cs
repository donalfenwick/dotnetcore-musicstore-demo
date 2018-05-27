using System;
using System.Threading.Tasks;
using Common.Models.Album;
using MusicStoreDemo.Common.Models.Album;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Common.Repositories{
    public interface IAlbumRepository{
        Task<AlbumDetail> GetAsync(int albumId);
        Task<AlbumList> ListAsync(int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<AlbumList> ListByArtistAsync(int artistId, int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<AlbumList> ListUserPurchasedAlbumsAsync(string userId, int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<UserAlbumOwnershipStatus> GetAlbumOwnershipStatusForUser(int albumId, string userId, PublishStatus statusFlags);
        Task<AlbumList> SearchAsync(string query, int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<AlbumList> ListByAlbumGroupKeyAsync(string albumGroupKey, int pageIndex, int pageSize, PublishStatus statusFlags);
        Task<AlbumDetail> AddAsync(Album album);
        Task<AlbumDetail> UpdateAsync(int albumId, Album album);
        Task PurchaseAsync(int albumId, string userId);
    }
}