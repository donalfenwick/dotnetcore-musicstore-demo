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
using MusicStoreDemo.Common.Models.Album;
using MusicStoreDemo.Common.Models.Enum;
using System.Linq.Expressions;
using Common.Models.Album;

namespace MusicStoreDemo.Common.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        public const int _maxPageSize = 50;

        private readonly MusicStoreDbContext _context;
        private readonly AlbumMapper _mapper;
        public AlbumRepository(MusicStoreDbContext context, AlbumMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AlbumDetail> GetAsync(int albumId)
        {
            var dbalbum = await _context.Albums
                                   .Include(x => x.Artist)
                                   .Include(x => x.AlbumGenres)
                                   .ThenInclude(x => x.Genre)
                                   .Include(x => x.Tracks)
                                   .SingleOrDefaultAsync(x => x.Id == albumId);
            if (dbalbum != null)
            {
                return _mapper.MapToDetailRep(dbalbum);
            }
            else
            {
                return null;
            }
        }

        public async Task<AlbumList> ListAsync(int pageIndex, int pageSize, PublishStatus statusFlags)
        {
            return await ListAsyncInternal(pageIndex, pageSize, statusFlags, searchExpression: null);
        }

        public async Task<AlbumList> ListByArtistAsync(int artistId, int pageIndex, int pageSize, PublishStatus statusFlags)
        {
            return await ListAsyncInternal(pageIndex, pageSize, statusFlags, searchExpression: x => x.ArtistId == artistId);
        }

        public async Task<AlbumList> SearchAsync(string query, int pageIndex, int pageSize, PublishStatus statusFlags)
        {
            // TODO: simple search condition on the albums title for now, could be improved with a smarter search algorithm in future
            return await ListAsyncInternal(pageIndex, pageSize, statusFlags,
                searchExpression: x => x.Title.Contains(query) || x.Artist.Name.Contains(query));
        }

        /// <summary>
        /// Gets a list of albums joined against the selected group and ordered by index
        /// </summary>
        public async Task<AlbumList> ListByAlbumGroupKeyAsync(string albumGroupKey, int pageIndex, int pageSize, PublishStatus statusFlags)
        {
            pageSize = Math.Min(pageSize, _maxPageSize);
            if(pageIndex < 0){
                pageIndex = 0;
            }
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.Map(statusFlags);
            var group = await _context.AlbumGroups.SingleOrDefaultAsync(g => g.Key == albumGroupKey);
            if (group == null)
            {
                throw new EntityNotFoundRepositoryException($"Group with key {albumGroupKey} not found");
            }

            AlbumList result = new AlbumList()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = 0
            };

            var dbItems = _context.AlbumGroups
                .Where(g => g.Key == albumGroupKey)
                .SelectMany(g => g.Items).OrderBy(x => x.PositionIndex)
                .Include(x => x.Album).ThenInclude(x => x.Artist)
                .Include(x => x.Album).ThenInclude(x => x.AlbumGenres)
                .ThenInclude(x => x.Genre)
                .Include(x => x.Album).ThenInclude(x => x.Tracks);

            result.TotalItems = await dbItems.CountAsync();

            var pageItems = dbItems.Skip(pageSize * pageIndex).Take(pageSize);
            var resultSet = await pageItems
                .ToListAsync();

            result.Items = resultSet.Select(a => _mapper.MapToDetailRep(a.Album)).ToList();

            return result;
        }

        private async Task<AlbumList> ListAsyncInternal(int pageIndex, int pageSize, PublishStatus statusFlags, Expression<Func<DbAlbum, bool>> searchExpression = null)
        {
            pageSize = Math.Min(pageSize, _maxPageSize);
            if(pageIndex < 0){
              pageIndex = 0;
            }
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            DbPublishedStatus contentStatusFlags = statusMapper.Map(statusFlags);
            AlbumList result = new AlbumList()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = 0
            };
            var dbItems = this._context.Albums.Where(a => (a.PublishStatus & contentStatusFlags) != 0);

            // apply an optional search expression on the list query to filter it
            if (searchExpression != null)
            {
                dbItems = dbItems.Where(searchExpression);
            }

            int totalRecords = await dbItems.CountAsync();
            dbItems = dbItems.Skip(pageSize * pageIndex).Take(pageSize);
            var dbAlbums = await dbItems
                                      .Include(x => x.Artist)
                                      .Include(x => x.AlbumGenres)
                                      .ThenInclude(x => x.Genre)
                                      .Include(x => x.Tracks)
                                      .ToListAsync();
            result.TotalItems = totalRecords;
            result.Items = dbAlbums.Select(a => _mapper.MapToDetailRep(a)).ToList();
            return result;
        }

        public async Task<AlbumDetail> AddAsync(Album album)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            var newEntity = new DbAlbum()
            {
                CreatedUtc = DateTime.Now,
                UpdatedUtc = DateTime.Now,
                Title = album.Title,
                DescriptionText = album.DescriptionText,
                PublishStatus = statusMapper.Map(album.PublishedStatus),
                Label = album.Label,
                Price = album.Price,
                Producer = album.Producer,
                ReleaseDate = album.ReleaseDate,
                TotalDurationInSeconds = album.TotalDurationInSeconds,
                ArtistId = album.ArtistId
            };
            foreach (AlbumTrack t in album.Tracks)
            {
                newEntity.Tracks.Add(
                    new DbTrack
                    {
                        CreatedUtc = DateTime.UtcNow,
                        DurationInSeconds = t.DurationInSeconds,
                        Title = t.Title,
                        TrackNumber = t.TrackNumber,
                        UpdatedUtc = DateTime.UtcNow
                    }
                );
            }
            foreach (string genre in album.Genres)
            {
                var dbGenre = await _context.Genres.SingleOrDefaultAsync(x => x.Name == genre);
                if (dbGenre != null)
                {
                    newEntity.AlbumGenres.Add((new DbAlbumGenre()
                    {
                        Genre = dbGenre
                    }));
                }
                else
                {
                    throw new RepositoryException($"The supplied genre '{genre}' does not exist.");
                }
            }
            if (album.CoverImageId.HasValue)
            {
                var img = await this._context.ImageResources.SingleOrDefaultAsync(x => x.Id == album.CoverImageId.Value);
                if (img != null)
                {
                    newEntity.AlbumCoverImage = img;
                }
                else
                {
                    throw new RepositoryException($"Invalid {nameof(album.CoverImageId)}, no image for ID");
                }
            }
            this._context.Albums.Add(newEntity);
            await this._context.SaveChangesAsync();

            return _mapper.MapToDetailRep(newEntity);
        }

        public async Task<AlbumDetail> UpdateAsync(int albumId, Album album)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            var dbalbum = await _context.Albums
                                   .Include(x => x.Artist)
                                   .Include(x => x.AlbumGenres)
                                   .ThenInclude(x => x.Genre)
                                   .Include(x => x.Tracks)
                                   .SingleOrDefaultAsync(x => x.Id == albumId);
            if (dbalbum != null)
            {
                dbalbum.Title = album.Title;
                dbalbum.DescriptionText = album.DescriptionText;
                dbalbum.PublishStatus = statusMapper.Map(album.PublishedStatus);
                dbalbum.Label = album.Label;
                dbalbum.Price = album.Price;
                dbalbum.Producer = album.Producer;
                dbalbum.ReleaseDate = album.ReleaseDate;
                dbalbum.TotalDurationInSeconds = album.TotalDurationInSeconds;
                dbalbum.ArtistId = album.ArtistId;

                // remove any existing genres that are not present on input model
                _context.AlbumGenres.RemoveRange(dbalbum.AlbumGenres.Where(x => x.AlbumId == albumId && !album.Genres.Contains(x.Genre.Name)));

                // add any genreas not already in the DB relationship 
                foreach (string newGenreName in album.Genres)
                {
                    if (dbalbum.AlbumGenres.FirstOrDefault(x => x.Genre.Name == newGenreName) == null)
                    {
                        var newGenreEntity = await _context.Genres.SingleOrDefaultAsync(x => x.Name == newGenreName);
                        if (newGenreEntity != null)
                        {
                            dbalbum.AlbumGenres.Add(new DbAlbumGenre()
                            {
                                Genre = newGenreEntity,
                                CreatedUtc = DateTime.UtcNow
                            });
                        }
                        else
                        {
                            // invalid genre
                            throw new RepositoryException($"The supplied genre '{newGenreName}' does not exist.");
                        }
                    }
                }

                if (album.CoverImageId.HasValue)
                {
                    var img = await this._context.ImageResources.SingleOrDefaultAsync(x => x.Id == album.CoverImageId.Value);
                    if (img != null)
                    {
                        dbalbum.AlbumCoverImage = img;
                    }
                    else
                    {
                        throw new RepositoryException($"Invalid {nameof(album.CoverImageId)}, no image for ID");
                    }
                }
                // find any tracks with database ID's and remove anything in the database with an ID not in this list
                List<int> existingTrackIds = album.Tracks.Where(t => t.TrackId.HasValue).Select(t => t.TrackId.Value).ToList();
                _context.Tracks.RemoveRange(dbalbum.Tracks.Where(x => x.AlbumId == albumId && !existingTrackIds.Contains(x.Id)));

                // now that any existing tracks which are not in the supplied model add or update based on the presence of a trackId
                foreach (var t in album.Tracks)
                {
                    DbTrack existingTrack = null;
                    if (t.TrackId.HasValue)
                    {
                        existingTrack = await _context.Tracks.SingleOrDefaultAsync(x => x.Id == t.TrackId.Value && x.AlbumId == albumId);
                    }
                    if (existingTrack != null)
                    {
                        // update track
                        existingTrack.TrackNumber = t.TrackNumber;
                        existingTrack.Title = t.Title;
                        existingTrack.DurationInSeconds = t.DurationInSeconds;
                        existingTrack.UpdatedUtc = DateTime.UtcNow;
                    }
                    else
                    {
                        //create new track
                        dbalbum.Tracks.Add(
                            new DbTrack
                            {
                                CreatedUtc = DateTime.UtcNow,
                                DurationInSeconds = t.DurationInSeconds,
                                Title = t.Title,
                                TrackNumber = t.TrackNumber,
                                UpdatedUtc = DateTime.UtcNow
                            }
                        );
                    }

                }

                await _context.SaveChangesAsync();

                return this._mapper.MapToDetailRep(dbalbum);
            }
            else
            {
                throw new EntityNotFoundRepositoryException($"Album with ID {albumId} not found");
            }
        }

        public async Task PurchaseAsync(int albumId, string userId)
        {
            DbAlbum dbalbum = await _context.Albums.SingleOrDefaultAsync(x => x.Id == albumId);
            DbUser user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (dbalbum != null && user != null)
            {
                var existingRecord = await _context.UserAlbums.SingleOrDefaultAsync(x => x.UserId == userId && x.AlbumId == albumId);
                if(existingRecord == null)
                {
                    _context.UserAlbums.Add(new DbUserPurchasedAlbum
                    {
                        CreatedUtc = DateTime.UtcNow,
                        AlbumId = albumId,
                        UserId = userId
                    });
                    await _context.SaveChangesAsync();
                }

            }
        }

        public async Task<AlbumList> ListUserPurchasedAlbumsAsync(string userId, int pageIndex, int pageSize, PublishStatus statusFlags){
            return await this.ListAsyncInternal(pageIndex, pageSize, statusFlags, x => x.UserPurchases.Any( p => p.UserId == userId));
        }
        
        public async Task<UserAlbumOwnershipStatus> GetAlbumOwnershipStatusForUser(int albumId, string userId, PublishStatus statusFlags){
            UserAlbumOwnershipStatus result = new UserAlbumOwnershipStatus();
            var dbRecord = await _context.UserAlbums.SingleOrDefaultAsync( x => x.UserId == userId && x.AlbumId == albumId);
            if(dbRecord != null){
                result.PurchaseDate = dbRecord.CreatedUtc;
                result.IsOwned = true;
            }
            return result;
        }
        
    }
}