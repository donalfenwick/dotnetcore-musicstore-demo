using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Models.AlbumGroup;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;

namespace MusicStoreDemo.Common.Repositories
{
    public class AlbumGroupRepository : IAlbumGroupRepository
    {
        private readonly MusicStoreDbContext _context;
        private readonly AlbumGroupMapper _mapper;
        public AlbumGroupRepository(MusicStoreDbContext context, AlbumGroupMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAlbumAsync(int groupId, int albumId)
        {
            DbAlbumGroupAlbumPosition entity = await _context.AlbumGroupListPositions.SingleOrDefaultAsync(x => x.GroupId == groupId && x.AlbumId == albumId);
            if (entity == null)
            {
                var group = await _context.AlbumGroups.SingleOrDefaultAsync(x => x.Id == groupId);
                if(group == null)
                {
                    throw new EntityNotFoundRepositoryException($"Album group with ID {groupId} not found");
                }
                var album = await _context.Albums.SingleOrDefaultAsync(x => x.Id == albumId);
                if (album == null)
                {
                    throw new EntityNotFoundRepositoryException($"Album with ID {albumId} not found");
                }
                var totalItemsInGroup = _context.AlbumGroups.SelectMany(x => x.Items).Count();
                entity = new DbAlbumGroupAlbumPosition
                {
                    Album = album,
                    CreatedUtc = DateTime.UtcNow,
                    Group = group,
                    PositionIndex = totalItemsInGroup + 1
                };
                await _context.AlbumGroupListPositions.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AlbumGroupDetail> AddAsync(AlbumGroup group)
        {
            var dbAlbumGroup = await _context.AlbumGroups.SingleOrDefaultAsync(x => x.Key == group.Key);
            if (dbAlbumGroup == null)
            {
                dbAlbumGroup = new DbAlbumGroup()
                {
                    Key = group.Key,
                    Name = group.Name,
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                };
                _context.AlbumGroups.Add(dbAlbumGroup);
                await _context.SaveChangesAsync();
                return new AlbumGroupDetail()
                {
                    Id = dbAlbumGroup.Id,
                    Key = dbAlbumGroup.Key,
                    Name = dbAlbumGroup.Name,
                    TotalAlbums = 0,
                    Created = new DateTime(dbAlbumGroup.CreatedUtc.Ticks, DateTimeKind.Utc),
                    Updated = new DateTime(dbAlbumGroup.UpdatedUtc.Ticks, DateTimeKind.Utc)
                };
            }
            else
            {
                throw new EntityAlreadyExistsRepositoryException($"A group with the key '{group.Key}' already exists");
            }
        }

        public async Task<AlbumGroupDetail> GetAsync(int groupId)
        {
            return await GetAsyncInternal(x => x.Id == groupId);
        }

        public async Task<AlbumGroupDetail> GetAsync(string key)
        {
            return await GetAsyncInternal(x => x.Key == key);
        }

        private async Task<AlbumGroupDetail> GetAsyncInternal(Expression<Func<DbAlbumGroup, bool>> findGroupExpression)
        {
            var dbAlbumGroup = await _context.AlbumGroups.SingleOrDefaultAsync(findGroupExpression);
            if (dbAlbumGroup != null)
            {
                var numAlbums = await _context.AlbumGroups.Where(g => g.Id == dbAlbumGroup.Id).SelectMany(x => x.Items).CountAsync(x => x.Album.PublishStatus == DbPublishedStatus.PUBLISHED);

                return new AlbumGroupDetail()
                {
                    Id = dbAlbumGroup.Id,
                    Key = dbAlbumGroup.Key,
                    Name = dbAlbumGroup.Name,
                    TotalAlbums = numAlbums,
                    Created = new DateTime(dbAlbumGroup.CreatedUtc.Ticks, DateTimeKind.Utc),
                    Updated = new DateTime(dbAlbumGroup.UpdatedUtc.Ticks, DateTimeKind.Utc)
                };
            }
            else
            {
                return null;
            }
        }


        public async Task<ICollection<AlbumGroupDetail>> ListAsync()
        {
            return await _context.AlbumGroups.Select(x => new AlbumGroupDetail
            {
                Created = new DateTime(x.CreatedUtc.Ticks, DateTimeKind.Utc),
                Key = x.Key,
                Id = x.Id,
                Name = x.Name,
                TotalAlbums = x.Items.Count(),
                Updated = new DateTime(x.UpdatedUtc.Ticks, DateTimeKind.Utc),
            }).ToListAsync();
        }

        public async Task RemoveAlbumAsync(int groupId, int albumId)
        {
            DbAlbumGroupAlbumPosition entity = await _context.AlbumGroupListPositions.SingleOrDefaultAsync(x => x.GroupId == groupId && x.AlbumId == albumId);
            if(entity != null)
            {
                _context.AlbumGroupListPositions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AlbumGroupDetail> UpdateAsync(int groupId, AlbumGroup group)
        {
            var dbAlbumGroup = await _context.AlbumGroups.SingleOrDefaultAsync(x => x.Id == groupId);
            if (dbAlbumGroup != null)
            {
                dbAlbumGroup.Name = group.Name;
                dbAlbumGroup.UpdatedUtc = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var numAlbums = await _context.AlbumGroups.Where(g => g.Id == dbAlbumGroup.Id).SelectMany(x => x.Items).CountAsync(x => x.Album.PublishStatus == DbPublishedStatus.PUBLISHED);

                return new AlbumGroupDetail()
                {
                    Id = dbAlbumGroup.Id,
                    Key = dbAlbumGroup.Key,
                    Name = dbAlbumGroup.Name,
                    TotalAlbums = numAlbums,
                    Created = new DateTime(dbAlbumGroup.CreatedUtc.Ticks, DateTimeKind.Utc),
                    Updated = new DateTime(dbAlbumGroup.UpdatedUtc.Ticks, DateTimeKind.Utc)
                };
            }
            else
            {
                throw new EntityNotFoundRepositoryException($"Album group with ID {groupId} not found");
            }
        }

        public async Task<AlbumGroupItemPositionListResult> GetOrderedAlbumListAsync(int groupId)
        {
            var resultSet = await _context.AlbumGroups.Where(x => x.Id == groupId)
                .SelectMany(x => x.Items)
                .Include(x => x.Album)
                .OrderBy(x => x.PositionIndex).ToListAsync();
            return new AlbumGroupItemPositionListResult
            {
                Items = resultSet.Select(x => new AlbumGroupItemPosition
                {
                    AlbumId = x.AlbumId,
                    AlbumTitle = x.Album.Title,
                    PositionIndex = x.PositionIndex
                }).ToList()
            };
        }

        public async Task SetOrderedAlbumListAsync(int groupId, List<int> albumIds)
        {
            var group = await _context.AlbumGroups.SingleOrDefaultAsync(x => x.Id == groupId);
            if (group != null)
            {
                _context.AlbumGroupListPositions.RemoveRange(_context.AlbumGroupListPositions.Where(x => x.GroupId == groupId));
                await _context.SaveChangesAsync();
                List<DbAlbumGroupAlbumPosition> newItems = new List<DbAlbumGroupAlbumPosition>();
                for (int i = 0; i < albumIds.Count; i++) {
                    newItems.Add(new DbAlbumGroupAlbumPosition
                    {
                        AlbumId = albumIds[i],
                        CreatedUtc = DateTime.UtcNow,
                        GroupId = groupId,
                        PositionIndex =i
                    });
                }
                await _context.AlbumGroupListPositions.AddRangeAsync(newItems);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundRepositoryException($"Album group with ID {groupId} not found");
            }
        }

		public async Task<List<AlbumGroup>> GetGroupsForAlbum(int albumId)
        {
			var album = await _context.Albums.SingleOrDefaultAsync(x => x.Id == albumId);
            if (album != null)
            {
				List<AlbumGroup> groups = _context.AlbumGroupListPositions
                      .Where(x => x.AlbumId == albumId)
                      .Include(x => x.Group)
                      .Select(x => new AlbumGroup
    					{
        					Id = x.Group.Id,
        					Key = x.Group.Key,
        					Name = x.Group.Name
    				    }).ToList();
				return groups;
            }
            else
            {
				throw new EntityNotFoundRepositoryException($"Album with ID {albumId} not found");
            }
        }

		public async Task SetGroupsForAlbum(int albumId, List<string> groupKeys){
			var album = await _context.Albums.SingleOrDefaultAsync(x => x.Id == albumId);
            if (album != null)
            {
				int totalItemsInGroup = _context.AlbumGroups.SelectMany(x => x.Items).Count();
    

				_context.AlbumGroupListPositions
				        .RemoveRange(_context.AlbumGroupListPositions.Where(x => x.AlbumId == albumId && !groupKeys.Contains(x.Group.Key)));

				foreach(string groupKey in groupKeys){
					var groupPosition = await _context.AlbumGroupListPositions.SingleOrDefaultAsync(p => p.AlbumId == albumId && p.Group.Key == groupKey);
					if(groupPosition == null){
						var group = await _context.AlbumGroups.SingleOrDefaultAsync(g => g.Key == groupKey);
						if (group != null)
						{
							_context.AlbumGroupListPositions.Add(new DbAlbumGroupAlbumPosition
							{
								AlbumId = albumId,
								CreatedUtc = DateTime.UtcNow,
								GroupId = group.Id,
								PositionIndex = totalItemsInGroup + 1
							});
							totalItemsInGroup++;
						}else{
							throw new EntityNotFoundRepositoryException($"Group with key {groupKey} not found");
						}
					}
				}
				await _context.SaveChangesAsync();
            }
            else
            {
                throw new EntityNotFoundRepositoryException($"Album with ID {albumId} not found");
            }
		}
    }
}
