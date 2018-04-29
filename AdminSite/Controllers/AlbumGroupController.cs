using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.AdminSite.Models.AlbumGroupViewModels;
using MusicStoreDemo.AdminSite.Models.Enums;
using MusicStoreDemo.AdminSite.Models.Mappers;
using MusicStoreDemo.Common.Models.AlbumGroup;
using MusicStoreDemo.Common.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.AdminSite.Controllers
{
    [Authorize(Policy = "isAdmin")]
    public class AlbumGroupController : Controller
    {
        
        private readonly IAlbumGroupRepository _albumGroupRepo;
        
        private readonly ILogger<AlbumGroupController> _log;
        private readonly AlbumGroupViewModelMapper _mapper;
        public AlbumGroupController(IAlbumGroupRepository albumGroupRepo, ILogger<AlbumGroupController> log)
        {
            _albumGroupRepo = albumGroupRepo;
            _log = log;
            _mapper = new AlbumGroupViewModelMapper();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ICollection<AlbumGroupDetail> result = await _albumGroupRepo.ListAsync();
            AlbumGroupIndexPageViewModel model = new AlbumGroupIndexPageViewModel();
            model.Items = result.Select(x => _mapper.Map(x)).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Reorder(int id)
        {
            ReorderAlbumGroupViewModel model = new ReorderAlbumGroupViewModel();
            model.GroupId = id;
            AlbumGroupDetail group = await _albumGroupRepo.GetAsync(id);
            AlbumGroupItemPositionListResult itemsResult = await _albumGroupRepo.GetOrderedAlbumListAsync(id);
            model.GroupName = group.Name;
            model.Items = itemsResult.Items.Select(x=> new ReorderAlbumGroupItemViewModel
            {
                AlbumId = x.AlbumId,
                AlbumTitle = x.AlbumTitle,
                PositionIndex = x.PositionIndex
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reorder(ReorderAlbumGroupViewModel model)
        {
            try
            {
                List<int> orderedAlbumIdList = model.Items.OrderBy(x => x.PositionIndex).Select(x => x.AlbumId).ToList();
                await _albumGroupRepo.SetOrderedAlbumListAsync(model.GroupId, orderedAlbumIdList);
                this.SetBootstrapPageAlert("Success", "Album group reordered", BootstrapAlertType.success);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _log.LogError(e, "Error reordering album group");
                this.SetBootstrapPageAlert("Error", "Error reordering album group", BootstrapAlertType.success);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            CreateAlbumGroupViewModel model = new CreateAlbumGroupViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateAlbumGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _albumGroupRepo.AddAsync(new AlbumGroup
                    {
                        Key = model.Key,
                        Name = model.Name
                    });
                    this.SetBootstrapPageAlert("Success", "Album group added", BootstrapAlertType.success);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    if(e is RepositoryException)
                    {
                        ModelState.AddModelError(nameof(model.Key), e.Message);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Key), "Unknown error");
                        _log.LogError("AddAlbumGroup", e, "Error adding group");
                    }
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EditAlbumGroupViewModel model = new EditAlbumGroupViewModel();
            var result = await _albumGroupRepo.GetAsync(id);
            if(result != null)
            {
                model.Key = result.Key;
                model.Name = result.Name;
                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAlbumGroupViewModel model)
        {
            AlbumGroupDetail group = await _albumGroupRepo.GetAsync(model.Id);
            if (group != null)
            {

                model.Key = group.Key; // key cant be overwritten
                if (ModelState.IsValid)
                {
                    try { 
                        group.Name = model.Name;
                    
                        await _albumGroupRepo.UpdateAsync(model.Id, group);
                        this.SetBootstrapPageAlert("Success", "Album group updated", BootstrapAlertType.success);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception e)
                    {
                        if (e is RepositoryException)
                        {
                            ModelState.AddModelError(nameof(model.Key), e.Message);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(model.Key), "Unknown error");
                            _log.LogError("EditAlbumGroup", e, "Error updating group");
                        }
                        return View(model);
                    }
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
