using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicStoreDemo.Common.Models.Album;
using MusicStoreDemo.Common.Models.AlbumGroup;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Common.Repositories{
    public interface IAlbumGroupRepository{
        Task<AlbumGroupDetail> GetAsync(int groupId);
        
        Task<AlbumGroupDetail> GetAsync(string key);
        Task<ICollection<AlbumGroupDetail>> ListAsync();
        Task<AlbumGroupDetail> AddAsync(AlbumGroup group);
        Task<AlbumGroupDetail> UpdateAsync(int groupId, AlbumGroup group);

        Task AddAlbumAsync(int groupId, int albumId);
        Task RemoveAlbumAsync(int groupId, int albumId);

        Task<AlbumGroupItemPositionListResult> GetOrderedAlbumListAsync(int groupId);
        Task SetOrderedAlbumListAsync(int groupId, List<int> albumIds);

		Task<List<AlbumGroup>> GetGroupsForAlbum(int albumId);
		Task SetGroupsForAlbum(int albumId, List<string> groupKeys);

    }
}