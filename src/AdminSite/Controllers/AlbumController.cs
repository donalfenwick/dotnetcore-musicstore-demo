using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicStoreDemo.AdminSite.Models;
using MusicStoreDemo.AdminSite.Models.AlbumViewModels;
using MusicStoreDemo.AdminSite.Models.Enums;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.AdminSite.Models.Mappers;
using MusicStoreDemo.Common.Models.Album;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Models.Image;
using MusicStoreDemo.Common.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.AdminSite.Controllers
{
	[Authorize(Policy = "isAdmin")]
    public class AlbumController : Controller
    {
        private const int _pageSize = 24;

        private readonly IAlbumRepository _albumRepo;
		private readonly IAlbumGroupRepository _albumGroupRepo;
        private readonly IArtistRepository _artistRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IGenreRepository _genreRepo;

        public AlbumController(
			IAlbumRepository albumRepo,
            IAlbumGroupRepository albumGroupRepo,
            IArtistRepository artistRepo, 
            IGenreRepository genreRepo, 
            IImageRepository imageRepo)
        {
            _albumRepo = albumRepo;
			_albumGroupRepo = albumGroupRepo;
            _artistRepo = artistRepo;
            _imageRepo = imageRepo;
            _genreRepo = genreRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]int page = 0, PublishStatus? status = null)
        {
            AlbumIndexViewModel model = new AlbumIndexViewModel();
            page = Math.Max(page, 0);
            PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
            if (status.HasValue)
            {
                flags = status.Value;
            }
            var result = await _albumRepo.ListAsync(page, _pageSize, flags);
            model.PageIndex = result.PageIndex;
            model.PageSize = result.PageSize;
            model.TotalPages = (int)Math.Ceiling(((double)result.TotalItems / (double)result.PageSize));

            AlbumViewModelMapper mapper = new AlbumViewModelMapper();
            model.Items.AddRange(result.Items.Select(i => mapper.Map(i)));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CreateAlbumViewModel model = new CreateAlbumViewModel();
            var genresResult = await _genreRepo.ListAsync(contentPublicationFlags: PublishStatus.PUBLISHED);
            model.Genres = genresResult.Genres.Select(g => new SelectableGenreViewModel
            {
                Name = g.Name,
                IsSelected = false
            }).ToList();
			var allGroups = await _albumGroupRepo.ListAsync();
			model.Groups = allGroups.Select(x => new CheckBoxListItem { Label = x.Name, Value = x.Key, IsSelected = false }).ToList();
            PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
            model.ArtistOptions = (await _artistRepo.ListNamesAsync(flags)).Select(x => 
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ArtistId
                }
            ).ToList();
			
            model.Tracks = new List<AlbumTrackViewModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAlbumViewModel model)
        {
            AlbumViewModelMapper mapper = new AlbumViewModelMapper();
            PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
            var artistNames = await _artistRepo.ListNamesAsync(flags);
            model.ArtistOptions = artistNames
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ArtistId
                }).ToList();
			List<CheckBoxListItem> groupOptions = (await _albumGroupRepo.ListAsync())
                    .Select(x => new CheckBoxListItem
                    {
                        Label = x.Name,
                        Value = x.Key,
                        IsSelected = model.Groups.Any(grp => grp.IsSelected && grp.Value == x.Key)
                    }).ToList();
            model.Groups = groupOptions;

            if (ModelState.IsValid)
            {
                int? createdImageId = null;
                if (model.CoverImage != null && model.CoverImage.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.CoverImage.CopyToAsync(ms);
                        ImageReferenceDetail imageRef = await _imageRepo.AddAsync(new ImageReferenceDetail
                        {
                            Data = ms.ToArray(),
                            FileName = model.CoverImage.FileName,
                            MimeType = model.CoverImage.ContentType
                        });
                        if (imageRef != null)
                        {
                            createdImageId = imageRef.Id;
                        }
                    }
                }


                var result = await _albumRepo.AddAsync(mapper.Map(model, createdImageId));

				await _albumGroupRepo.SetGroupsForAlbum(result.Id, model.Groups.Where(g => g.IsSelected).Select(g => g.Value).ToList());

                this.SetBootstrapPageAlert("Success", "Album added", BootstrapAlertType.success);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AlbumViewModelMapper mapper = new AlbumViewModelMapper();
            EditAlbumViewModel model = new EditAlbumViewModel();
           

            var genresResult = await _genreRepo.ListAsync(contentPublicationFlags: PublishStatus.PUBLISHED);
            
			var allGroups = await _albumGroupRepo.ListAsync();
			var albumGroupKeys = (await _albumGroupRepo.GetGroupsForAlbum(id)).Select(x => x.Key);


            AlbumDetail album = await _albumRepo.GetAsync(id);
            if (album != null)
            {
                PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
                ICollection<ArtistNameRef> artistNames = await _artistRepo.ListNamesAsync(flags);
                model = mapper.Map(album, artistNames, genresResult.Genres);
				model.Groups = allGroups.Select(x => new CheckBoxListItem
                {
                    Label = x.Name,
                    Value = x.Key,
                    IsSelected = (albumGroupKeys.Contains(x.Key))
                }).ToList();

                return View(model);
            }
            else
            {
                // todo: show error message
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAlbumViewModel model)
        {
            AlbumViewModelMapper mapper = new AlbumViewModelMapper();
            PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
            var artistNames = await _artistRepo.ListNamesAsync(flags);
            model.ArtistOptions = artistNames
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == model.ArtistId
                }).ToList();

			List<CheckBoxListItem> groupOptions = (await _albumGroupRepo.ListAsync())
				.Select( x => new CheckBoxListItem { 
				Label = x.Name, 
				Value = x.Key, 
				IsSelected = model.Groups.Any(grp => grp.IsSelected && grp.Value == x.Key)
			}).ToList();
			model.Groups = groupOptions;


            AlbumDetail album = await _albumRepo.GetAsync(model.Id);
            if (album != null)
            {
                // set non postback properties 
                model.CoverImageId = album.CoverImageId;
                model.Created = album.CreatedUtc;
                model.Updated = album.UpdatedUtc;
                model.PublishedStatus = album.PublishedStatus;
                if (ModelState.IsValid)
                {
                    int? createdCoverImageId = null;
                    if (model.CoverImage != null && model.CoverImage.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await model.CoverImage.CopyToAsync(ms);
                            ImageReferenceDetail imageRef = await _imageRepo.AddAsync(new ImageReferenceDetail
                            {
                                Data = ms.ToArray(),
                                FileName = model.CoverImage.FileName,
                                MimeType = model.CoverImage.ContentType
                            });
                            if (imageRef != null)
                            {
                                createdCoverImageId = imageRef.Id;
                            }
                        }
                    }

                    album = mapper.MapOnTo(model, album, createdCoverImageId);
                    await _albumRepo.UpdateAsync(album.Id, album);
					await _albumGroupRepo.SetGroupsForAlbum(model.Id, model.Groups.Where(g => g.IsSelected).Select(g => g.Value).ToList());

                    this.SetBootstrapPageAlert("Success", "Artist updated", BootstrapAlertType.success);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                // todo: show error message
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
