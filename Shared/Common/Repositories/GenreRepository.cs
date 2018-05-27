using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreDemo.Common.Models.Genre;
using MusicStoreDemo.Database;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Mappers;

namespace MusicStoreDemo.Common.Repositories{
    public class GenreRepository : IGenreRepository{

        private readonly MusicStoreDbContext _context;
        public GenreRepository(MusicStoreDbContext context){
            _context = context;
        }
        
        public  async Task<GenreDetail> GetAsync(string genre, PublishStatus? contentPublicationFlags = null)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.GetAllDbFlags();
            if (contentPublicationFlags.HasValue)
            {
                contentStatusFlags = statusMapper.Map(contentPublicationFlags.Value);
            }
            var dbGenre = await _context.Genres.SingleOrDefaultAsync(x => x.Name == genre);
            if(dbGenre != null){
                var numAlbums = await _context.Genres.Where(g => g.Name == genre).SelectMany(x => x.AlbumGenres).CountAsync(x => x.Album.PublishStatus == DbPublishedStatus.PUBLISHED);
                var numArtists = await _context.Genres.Where(g => g.Name == genre).SelectMany(x => x.ArtistGenres).CountAsync(x => x.Artist.PublishStatus == DbPublishedStatus.PUBLISHED);
                GenreDetail result = new GenreDetail()
                {
                    Name = dbGenre.Name,
                    TotalAlbums = numAlbums,
                    TotalArtists = numArtists,
                    Created = new DateTime(dbGenre.CreatedUtc.Ticks, DateTimeKind.Utc)
                };
                return result;
            }else{
                return null;
            }
        }

        public async Task<GenreList> ListAsync(PublishStatus? contentPublicationFlags = null)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.GetAllDbFlags();
            if (contentPublicationFlags.HasValue)
            {
                contentStatusFlags = statusMapper.Map(contentPublicationFlags.Value);
            }
            List<GenreDetail> genres = await _context.Genres.Select(g => new GenreDetail{
                Name = g.Name,
                Created = new DateTime(g.CreatedUtc.Ticks, DateTimeKind.Utc),
                TotalAlbums = g.AlbumGenres.Count(x => (x.Album.PublishStatus & contentStatusFlags) != 0),
                TotalArtists = g.ArtistGenres.Count(x => (x.Artist.PublishStatus & contentStatusFlags) != 0)
            }).ToListAsync();
            GenreList result = new GenreList()
            {
                Genres = genres
            };
            return result;
        }

        public async Task<GenreDetail> AddAsync(string genre){
            var dbGenre = await _context.Genres.SingleOrDefaultAsync(x => x.Name == genre);
            if(dbGenre == null){
                dbGenre = new DbGenre()
                {
                    Name = genre,
                    CreatedUtc = DateTime.UtcNow
                };
                _context.Genres.Add(dbGenre);
                await _context.SaveChangesAsync();
                return new GenreDetail()
                {
                    Name = dbGenre.Name,
                    TotalAlbums = 0,
                    TotalArtists = 0,
                    Created = new DateTime(dbGenre.CreatedUtc.Ticks, DateTimeKind.Utc)
                };
                
            }else{
                var numAlbums = await _context.Genres.Where(g => g.Name == genre).SelectMany(x => x.AlbumGenres).CountAsync(x=> x.Album.PublishStatus == DbPublishedStatus.PUBLISHED);
                var numArtists = await _context.Genres.Where(g => g.Name == genre).SelectMany(x => x.ArtistGenres).CountAsync(x => x.Artist.PublishStatus == DbPublishedStatus.PUBLISHED);
                return new GenreDetail()
                {
                    Name = dbGenre.Name,
                    TotalAlbums = numAlbums,
                    TotalArtists = numArtists,
                    Created = new DateTime(dbGenre.CreatedUtc.Ticks, DateTimeKind.Utc)
                };
            }
        }

        public async Task DeleteAsync(string genre)
        {
            var dbGenre = await _context.Genres
                .Include(x=>x.AlbumGenres)
                .Include(x=>x.ArtistGenres)
                .SingleOrDefaultAsync(x => x.Name == genre);
            _context.RemoveRange(dbGenre.ArtistGenres);
            _context.RemoveRange(dbGenre.AlbumGenres);
            _context.Remove(dbGenre);
            await _context.SaveChangesAsync();
        }
    }
}