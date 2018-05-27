using MusicStoreDemo.Common.Models.Image;
using MusicStoreDemo.Database;
using System;
using System.Threading.Tasks;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace MusicStoreDemo.Common.Repositories
{
    public class ImageRepository : IImageRepository
    {

        private readonly MusicStoreDbContext _context;
        public ImageRepository(MusicStoreDbContext context){
            _context = context;
        }

        public async Task<ImageReferenceDetail> GetAsync(int id){
            var imageRecord = await _context.ImageResources
                                   .SingleOrDefaultAsync(x => x.Id == id);
            if (imageRecord != null)
            {
                return new ImageReferenceDetail(){
                    Id = imageRecord.Id,
                    FileName = imageRecord.Filename,
                    MimeType = imageRecord.MimeType,
                    Data = imageRecord.Data
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<ImageReferenceDetail> AddAsync(ImageReferenceDetail image){
            var dbImage = new DbImageResource()
            {
                CreatedUtc = DateTime.Now,
                UpdatedUtc = DateTime.Now,
                Filename = image.FileName,
                MimeType = image.MimeType,
                Data = image.Data
            };
            this._context.ImageResources.Add(dbImage);
            await this._context.SaveChangesAsync();

            image.Id = dbImage.Id;

            return image;
        }
        
    }
}
