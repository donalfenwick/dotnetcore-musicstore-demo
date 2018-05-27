using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStoreDemo.AdminSite.Models.ArtistViewModels;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.Common.Models.Artist;
using MusicStoreDemo.Common.Models.Image;
using MusicStoreDemo.Common.Repositories;
using MusicStoreDemo.Database.Entities;
using System.Collections;
using MusicStoreDemo.AdminSite.Models.Enums;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.AdminSite.Models.Mappers;

namespace MusicStoreDemo.AdminSite.Controllers
{
    [Authorize(Policy = "isAdmin")]
    public class ArtistController : Controller
    {
        private const int _pageSize = 24;

        private readonly IArtistRepository _artistRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IGenreRepository _genreRepo;

        public ArtistController(IArtistRepository artistRepo, IGenreRepository genreRepo, IImageRepository imageRepo)
        {
            _artistRepo = artistRepo;
            _imageRepo = imageRepo;
            _genreRepo = genreRepo;
        }

        public async Task<IActionResult> Index([FromQuery]int page = 0, PublishStatus? status = null)
        {
            ArtistIndexViewModel model = new ArtistIndexViewModel();
            page = Math.Max(page, 0);
            PublishStatus flags = PublishStatus.PUBLISHED | PublishStatus.UNPUBLISHED;
            if (status.HasValue)
            {
                flags = status.Value;
            }
            var result = await _artistRepo.ListAsync(page, _pageSize, flags);
            model.PageIndex = result.PageIndex;
            model.PageSize = result.PageSize;
            model.TotalPages = (int)Math.Ceiling(((double)result.TotalItems / (double)result.PageSize));

            ArtistViewModelMapper mapper = new ArtistViewModelMapper();
            model.Items.AddRange(result.Items.Select(i => mapper.Map(i)));
            
            return View(model);
        }

        public async Task<IActionResult> Details([FromQuery]int id)
        {
            Artist artist = await _artistRepo.GetAsync(id);
            if (artist != null)
            {
                return View(new ArtistDetailsViewModel
                {
                    Artist = artist
                });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            CreateArtistViewModel model = new CreateArtistViewModel();
            var genresResult = await _genreRepo.ListAsync(contentPublicationFlags: PublishStatus.PUBLISHED);
            model.Genres = genresResult.Genres.Select(g => new SelectableGenreViewModel
            {
                Name = g.Name,
                IsSelected = false
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateArtistViewModel model)
        {
            if (ModelState.IsValid)
            {
                int? createdBioImageId = null;
                if(model.BioImage != null && model.BioImage.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.BioImage.CopyToAsync(ms);
                        ImageReferenceDetail imageRef = await _imageRepo.AddAsync(new ImageReferenceDetail
                        {
                            Data = ms.ToArray(),
                            FileName = model.BioImage.FileName,
                            MimeType = model.BioImage.ContentType
                        });
                        if(imageRef != null)
                        {
                            createdBioImageId = imageRef.Id;
                        }
                    }
                }

                var result = await _artistRepo.AddAsync(new Artist
                {
                    PublishedStatus = PublishStatus.UNPUBLISHED,
                    Name = model.Name,
                    BioText = model.BioText,
                    Genres = model.Genres.Where(g => g.IsSelected).Select(g => g.Name).ToList(),
                    BioImageId = createdBioImageId
                });
                this.SetBootstrapPageAlert("Success", "Artist added", BootstrapAlertType.success);
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
            EditArtistViewModel model = new EditArtistViewModel();
            var genresResult = await _genreRepo.ListAsync(contentPublicationFlags: PublishStatus.PUBLISHED);
            model.Genres = genresResult.Genres.Select(g => new SelectableGenreViewModel
            {
                Name = g.Name,
                IsSelected = false
            }).ToList();

            ArtistDetail artist = await _artistRepo.GetAsync(id);
            if(artist != null)
            {
                model.Id = artist.Id;
                model.Name = artist.Name;
                model.BioText = artist.BioText;
                model.BioImageId = artist.BioImageId;
                model.Created = artist.CreatedUtc;
                model.Updated = artist.UpdatedUtc;
                model.PublishedStatus = artist.PublishedStatus;
                model.Genres.Where(g => artist.Genres.Contains(g.Name)).ToList().ForEach(x => x.IsSelected = true);
               
                return View(model);
            }
            else
            {
                // todo: show error message
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditArtistViewModel model)
        {
            ArtistDetail artist = await _artistRepo.GetAsync(model.Id);
            if (artist != null)
            {
                // set non postback properties 
                model.BioImageId = artist.BioImageId;
                model.Created = artist.CreatedUtc;
                model.Updated = artist.UpdatedUtc;
                model.PublishedStatus = artist.PublishedStatus;
                if (ModelState.IsValid)
                {
                    int? createdBioImageId = null;
                    if (model.BioImage != null && model.BioImage.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await model.BioImage.CopyToAsync(ms);
                            ImageReferenceDetail imageRef = await _imageRepo.AddAsync(new ImageReferenceDetail
                            {
                                Data = ms.ToArray(),
                                FileName = model.BioImage.FileName,
                                MimeType = model.BioImage.ContentType
                            });
                            if (imageRef != null)
                            {
                                createdBioImageId = imageRef.Id;
                            }
                        }
                    }

                    artist.Name = model.Name;
                    artist.BioText = model.BioText;
                    artist.UpdatedUtc = DateTime.UtcNow;
                    artist.Genres = model.Genres.Where(g => g.IsSelected).Select(g => g.Name).ToList();
                    if (createdBioImageId.HasValue)
                    {
                        artist.BioImageId = createdBioImageId.Value;
                    }
                    await _artistRepo.UpdateAsync(artist.Id, artist);
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ArtistDetail artist = await _artistRepo.GetAsync(id);
            if (artist != null)
            {
                artist.PublishedStatus = PublishStatus.DELETED;
                await _artistRepo.UpdateAsync(artist.Id, artist);
                this.SetBootstrapPageAlert("Success", "Artist deleted", BootstrapAlertType.success);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.SetBootstrapPageAlert("Error", "Artist was not deleted", BootstrapAlertType.danger);
                return RedirectToAction(nameof(Index));
            }
        }

    }
}