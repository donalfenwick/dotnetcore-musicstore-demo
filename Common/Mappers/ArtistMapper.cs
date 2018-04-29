using System;
using System.Linq;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Database.Entities;

namespace MusicStoreDemo.Common.Mappers
{
    public class ArtistMapper
    {
        public ArtistMapper()
        {
        }

        public ArtistDetail MapToDetailRep(DbArtist a)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            if (a == null)
            {
                throw new NullReferenceException($"A null value was passed to the mapper. Expected an object of type {nameof(DbArtist)}");
            }
            return new ArtistDetail
            {
                Id = a.Id,
                Name = a.Name,
                BioText = a.BioText,
                BioImageId = a.BioImageId,
                BioImageUrl = (a.BioImageId.HasValue ? $"/api/image/{a.BioImageId.Value}" : null),
                Genres = a.ArtistGenres.Select(g => g.Genre.Name).ToList(),
                CreatedUtc = new DateTime(a.CreatedUtc.Ticks, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(a.UpdatedUtc.Ticks, DateTimeKind.Utc),
                PublishedStatus = statusMapper.Map(a.PublishStatus)
            };
        }
    }
}
