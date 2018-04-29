using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreDemo.AdminSite.Models.AlbumViewModels;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.Common.Models.Album;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Models.Genre;

namespace MusicStoreDemo.AdminSite.Models.Mappers
{
    public class AlbumViewModelMapper
    {
        public AlbumListItemViewModel Map(AlbumDetail a){
            return new AlbumListItemViewModel
            {
                Id = a.Id,
                Created = a.CreatedUtc,
                Status = a.PublishedStatus,
                Title = a.Title,
                ArtistName = a.ArtistName,
                ArtistId = a.ArtistId,
                Updated = a.UpdatedUtc
            };
        }

        public AlbumTrackViewModel Map(AlbumTrack a)
        {
            return new AlbumTrackViewModel
            {
                Id = a.TrackId,
                DurationInSec = a.DurationInSeconds,
                TrackNumber = a.TrackNumber,
                Title = a.Title
            };
        }

        public AlbumTrack Map(AlbumTrackViewModel a)
        {
            return new AlbumTrack
            {
                TrackId = a.Id,
                DurationInSeconds = a.DurationInSec,
                TrackNumber = a.TrackNumber,
                Title = a.Title
            };
        }

        public Album Map(CreateAlbumViewModel model, int? coverImageId)
        {
            return new Album
            {
                PublishedStatus = PublishStatus.UNPUBLISHED,
                Title = model.Title,
                DescriptionText = model.DescriptionText,
                Genres = model.Genres.Where(g => g.IsSelected).Select(g => g.Name).ToList(),
                CoverImageId = coverImageId,
                ArtistId = model.ArtistId,
                Label = model.Label,
                Price = model.Price,
                Producer = model.Producer,
                ReleaseDate = model.ReleaseDate,
                TotalDurationInSeconds = model.Tracks.Sum(t => t.DurationInSec),
                Tracks = model.Tracks.Select(t => this.Map(t)).ToList()
            };
        }

        public EditAlbumViewModel Map(AlbumDetail album, ICollection<ArtistNameRef> artistNames, ICollection<GenreDetail> genres)
        {
            GenreViewModelMapper genreMapper = new GenreViewModelMapper();
            EditAlbumViewModel model = new EditAlbumViewModel();
            model.Id = album.Id;
            model.ArtistId = album.ArtistId;
            model.Title = album.Title;
            model.DescriptionText = album.DescriptionText;
            model.Label = album.Label;
            model.Price = album.Price;
            model.Producer = album.Producer;
            model.ReleaseDate = album.ReleaseDate;
            model.CoverImageId = album.CoverImageId;
            model.Created = album.CreatedUtc;
            model.Updated = album.UpdatedUtc;
            model.PublishedStatus = album.PublishedStatus;
            

            model.Tracks = album.Tracks.Select(x => this.Map(x)).ToList();
            model.ArtistOptions = artistNames
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ArtistId
                }).ToList();

            model.Genres = genres.Select(g => genreMapper.Map(g)).ToList();
            model.Genres.Where(g => album.Genres.Contains(g.Name)).ToList().ForEach(x => x.IsSelected = true);

            return model;
        }

        public AlbumDetail MapOnTo(EditAlbumViewModel model, AlbumDetail existingObject, int? coverImageId)
        {
            existingObject.Title = model.Title;
            existingObject.DescriptionText = model.DescriptionText;
            existingObject.Genres = model.Genres.Where(g => g.IsSelected).Select(g => g.Name).ToList();
            if (coverImageId.HasValue)
            {
                existingObject.CoverImageId = coverImageId.Value;
            }
            existingObject.Label = model.Label;
            existingObject.Price = model.Price;
            existingObject.Producer = model.Producer;
            existingObject.ReleaseDate = model.ReleaseDate;
            existingObject.Tracks = model.Tracks.Select(x => this.Map(x)).ToList();
            return existingObject;
        }
    }
}
