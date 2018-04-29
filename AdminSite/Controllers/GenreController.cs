using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.AdminSite.Models.Enums;
using MusicStoreDemo.AdminSite.Models.GenreViewModels;
using MusicStoreDemo.AdminSite.Models.Mappers;
using MusicStoreDemo.Common.Models.Enum;
using MusicStoreDemo.Common.Models.Genre;
using MusicStoreDemo.Common.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.AdminSite.Controllers
{
	[Authorize(Policy = "isAdmin")]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepo;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreRepository genreRepo, ILogger<GenreController> logger)
        {
            _genreRepo = genreRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            GenreViewModelMapper mapper = new GenreViewModelMapper();
            GenreIndexPageViewModel model = new GenreIndexPageViewModel();
            //GenreList genres = await _genreRepo.ListAsync(PublishStatus.UNPUBLISHED|PublishStatus.PUBLISHED|PublishStatus.ARCHIVED);
            GenreList genres = await _genreRepo.ListAsync();
            model.Genres = genres.Genres.Select(x => mapper.MapToViewModel(x)).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new CreateGenreViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateGenreViewModel model)
        {
            if (ModelState.IsValid)
            {
                GenreDetail existingGenre = await _genreRepo.GetAsync(model.Name);
                if (existingGenre == null)
                {
                    try
                    {
                        await _genreRepo.AddAsync(model.Name);
                        this.SetBootstrapPageAlert("Success", "New genre added", BootstrapAlertType.success);
                        return RedirectToAction(nameof(Index));
                    }catch(Exception e)
                    {
                        _logger.LogError(e, "Error creating genre");
                        ModelState.AddModelError(nameof(model.Name), "Error adding genre to the database");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Name), "A genre with this name already exists");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Remove(string name)
        {
            GenreViewModelMapper mapper = new GenreViewModelMapper();
            GenreDetail existingGenre = await _genreRepo.GetAsync(name);
            GenreItemViewModel model = mapper.MapToViewModel(existingGenre);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Remove(GenreItemViewModel model)
        {
            GenreViewModelMapper mapper = new GenreViewModelMapper();
            try
            {
                await _genreRepo.DeleteAsync(model.Name);
                this.SetBootstrapPageAlert("Success", $"Deleted genre '{model.Name}' from the database", BootstrapAlertType.success);
                return RedirectToAction(nameof(Index));
            }catch(Exception e)
            {
                _logger.LogError(e, "Error deleting genre");
                ModelState.AddModelError(nameof(model.Name), "Error deleting genre from the database");
                GenreDetail existingGenre = await _genreRepo.GetAsync(model.Name);
                model = mapper.MapToViewModel(existingGenre);
            }
            return View(model);
        }
    }
}
