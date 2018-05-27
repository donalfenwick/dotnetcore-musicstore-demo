using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreDemo.Database;
using MusicStoreDemo.Common.Models;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Common.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        public const int _maxPageSize = 50;

        private readonly MusicStoreDbContext _context;
        private readonly ArtistMapper _mapper;
        public ArtistRepository(MusicStoreDbContext context, ArtistMapper mapper){
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArtistDetail> GetAsync(int artistId){
            var dbartist = await _context.Artists
                                   .Include(x => x.ArtistGenres)
                                   .ThenInclude(x => x.Genre)
                                   .SingleOrDefaultAsync(x => x.Id == artistId);
            if(dbartist != null){
                return _mapper.MapToDetailRep(dbartist);
            }else{
                return null;
            }
        }

        public async Task<ArtistList> ListAsync(int pageIndex, int pageSize, PublishStatus statusFlags = PublishStatus.PUBLISHED)
        {
            pageSize = Math.Min(pageSize, _maxPageSize);
            ArtistList result = new ArtistList(){
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = 0
            };
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.Map(statusFlags);
            var artistsQuery = this._context.Artists.AsQueryable();
            // bitwise & the status flags and check the result is not 0 to get any records matching flags
            artistsQuery = artistsQuery.Where(a => (a.PublishStatus & contentStatusFlags) != 0);
            
            int totalRecords = await artistsQuery.CountAsync();
            artistsQuery = artistsQuery.Skip(pageSize * pageIndex).Take(pageSize);

            var dbArtists = await artistsQuery
                                      .Include(x => x.ArtistGenres)
                                      .ThenInclude(x => x.Genre)
                                      .ToListAsync();
            result.TotalItems = totalRecords;
            result.Items = dbArtists.Select(a => _mapper.MapToDetailRep(a)).ToList();
            return result;
        }

        public async Task<ICollection<ArtistNameRef>> ListNamesAsync(PublishStatus statusFlags){
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.Map(statusFlags);
            var artistsQuery = this._context.Artists.AsQueryable();
            // bitwise & the status flags and check the result is not 0 to get any records matching flags
            artistsQuery = artistsQuery.Where(a => (a.PublishStatus & contentStatusFlags) != 0);

            var result = await artistsQuery.Select( x=> new ArtistNameRef{
                Id=  x.Id,
                Name = x.Name,
                Status = statusMapper.Map(x.PublishStatus)
            }).ToListAsync();
            return result;
        }

        public async Task<ArtistDetail> AddAsync(Artist artist)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            var newArtistEntity = new DbArtist()
            {
                CreatedUtc = DateTime.Now,
                UpdatedUtc = DateTime.Now,
                Name = artist.Name,
                BioText = artist.BioText,
                PublishStatus = statusMapper.Map(artist.PublishedStatus)
            };
            foreach (string genre in artist.Genres)
            {
                var dbGenre = await _context.Genres.SingleOrDefaultAsync(x => x.Name == genre);
                if (dbGenre != null)
                {
                    newArtistEntity.ArtistGenres.Add((new DbArtistGenre()
                    {
                        Genre = dbGenre
                    }));
                }
                else
                {
                    throw new RepositoryException($"The supplied genre '{genre}' does not exist.");
                }
            }
            if(artist.BioImageId.HasValue){
                var img = await this._context.ImageResources.SingleOrDefaultAsync(x => x.Id == artist.BioImageId.Value);
                if(img!= null){
                    newArtistEntity.BioImage = img;
                }else{
                    throw new RepositoryException($"Invalid {nameof(artist.BioImageId)}, no image for ID");
                }
            }
            this._context.Artists.Add(newArtistEntity);
            await this._context.SaveChangesAsync();

            return _mapper.MapToDetailRep(newArtistEntity);
        }

        public async Task<ArtistDetail> UpdateAsync(int artistId, Artist artist)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            var dbartist = await _context.Artists
                                      .Include(x => x.ArtistGenres)
                                      .ThenInclude(x => x.Genre)
                                      .SingleOrDefaultAsync(x => x.Id == artistId);
            if (dbartist != null)
            {
                dbartist.Name = artist.Name;
                dbartist.UpdatedUtc = DateTime.UtcNow;
                dbartist.BioText = artist.BioText;
                dbartist.PublishStatus = statusMapper.Map(artist.PublishedStatus);
                // remove any existing genres that are not present on input model
                _context.ArtistGenres.RemoveRange(dbartist.ArtistGenres.Where(x => x.ArtistId == artistId && !artist.Genres.Contains(x.Genre.Name)));

                // add any genreas not already in the DB relationship 
                foreach(string newGenreName in artist.Genres){
                    if(dbartist.ArtistGenres.FirstOrDefault(x => x.Genre.Name == newGenreName) == null){
                        var newGenreEntity = await _context.Genres.SingleOrDefaultAsync(x => x.Name == newGenreName);
                        if (newGenreEntity != null)
                        {
                            dbartist.ArtistGenres.Add(new DbArtistGenre()
                            {
                                Genre = newGenreEntity,
                                CreatedUtc = DateTime.UtcNow
                            });
                        }else{
                            // invalid genre
                            throw new RepositoryException($"The supplied genre '{newGenreName}' does not exist.");
                        }
                    }
                }

                if(artist.BioImageId.HasValue){
                    var img = await this._context.ImageResources.SingleOrDefaultAsync(x => x.Id == artist.BioImageId.Value);
                    if(img!= null){
                        dbartist.BioImage = img;
                    }else{
                        throw new RepositoryException($"Invalid {nameof(artist.BioImageId)}, no image for ID");
                    }
                }

                await _context.SaveChangesAsync();

                return this._mapper.MapToDetailRep(dbartist);
            }else{
                throw new EntityNotFoundRepositoryException($"Artist with ID {artistId} not found");
            }
        }
    }
}