using System;
using System.Threading.Tasks;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Models.Genre;

namespace MusicStoreDemo.Common.Repositories{
    public interface IGenreRepository{
        Task<GenreDetail> GetAsync(string genre, PublishStatus? contentPublicationFlags = null);
        Task<GenreList> ListAsync(PublishStatus? contentPublicationFlags = null);
        Task<GenreDetail> AddAsync(string genre);
        Task DeleteAsync(string genre);
    }
}