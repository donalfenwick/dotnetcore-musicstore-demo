using System;
using System.Collections.Generic;
using System.Linq;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Common.Models;
using MusicStoreDemo.Common.Models.Album;
namespace MusicStoreDemo.Common.Mappers
{
    public class AlbumMapper
    {
        public AlbumMapper()
        {
        }

        public AlbumDetail MapToDetailRep(DbAlbum a)
        {
            PublishedStatusEnumMapper statusMapper = new PublishedStatusEnumMapper();
            if (a == null)
            {
                throw new NullReferenceException($"A null value was passed to the mapper. Expected an object of type {nameof(DbAlbum)}");
            }
            List<AlbumTrack> tracks = a.Tracks
                .OrderByDescending(x => x.TrackNumber )
                .Select( x=> new AlbumTrack()
                {
                    Title = x.Title,
                    DurationInSeconds = x.DurationInSeconds,
                    TrackNumber = x.TrackNumber,
                    TrackId = x.Id
                }).OrderBy(x => x.TrackNumber).ToList();

            var album = new AlbumDetail
            {
                Id = a.Id,
                Title = a.Title,
                DescriptionText = a.DescriptionText,
                CoverImageId = a.AlbumCoverImageId,
                Price = a.Price,
                Producer = a.Producer,
                Label = a.Label,
                ReleaseDate = a.ReleaseDate,
                TotalDurationInSeconds = a.TotalDurationInSeconds,
                Tracks = tracks,
                CoverImageUrl = (a.AlbumCoverImageId.HasValue ? $"/api/image/{a.AlbumCoverImageId.Value}" : null),
                Genres = a.AlbumGenres.Select(g => g.Genre.Name).ToList(),
                CreatedUtc = new DateTime(a.CreatedUtc.Ticks, DateTimeKind.Utc),
                UpdatedUtc = new DateTime(a.UpdatedUtc.Ticks, DateTimeKind.Utc),
                PublishedStatus = statusMapper.Map(a.PublishStatus),
                ArtistId = a.ArtistId
            };

            if(a.Artist != null){
                album.ArtistName = a.Artist.Name;
            }


            return album;
        }
    }
}
