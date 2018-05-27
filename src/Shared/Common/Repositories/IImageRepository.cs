using MusicStoreDemo.Common.Models.Image;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreDemo.Common.Repositories
{
    public interface IImageRepository
    {
        Task<ImageReferenceDetail> GetAsync(int id);

        Task<ImageReferenceDetail> AddAsync(ImageReferenceDetail image);
        
    }
}
