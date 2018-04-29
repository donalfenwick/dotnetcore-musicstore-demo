using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Models;
using MusicStoreDemo.Common.Models.AlbumGroup;
using MusicStoreDemo.Common.Repositories;

namespace MusicStoreDemo.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/AlbumGroup")]
    public class AlbumGroupController : ControllerBase
    {
        private readonly IAlbumGroupRepository _repository;
        private readonly ILogger<AlbumGroupController> _logger;

        public AlbumGroupController(IAlbumGroupRepository repository, ILogger<AlbumGroupController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _repository.ListAsync();
                return Ok(result);
            }
            catch (EntityNotFoundRepositoryException e)
            {
                return NotFound(new ApiErrorRep(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError("ListAlbum", e, "Error listing albums");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpGet("{key}")]
        [ProducesResponseType(typeof(AlbumGroupDetail), 200)]
        public async Task<IActionResult> Get(string key)
        {
            try
            {
                AlbumGroupDetail group = await _repository.GetAsync(key);
                if (group != null)
                {
                    return Ok(group);
                }
                else
                {
                    return NotFound(new ApiErrorRep($"Album group with key {key} was nto found"));
                }
            }
            catch (RepositoryException e)
            {
                return BadRequest(new ApiErrorRep(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError("GetArtist", e, "Error getting album");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }


        [HttpPost("")]
        [ProducesResponseType(typeof(AlbumGroupDetail), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody]AlbumGroup albumGroup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AlbumGroupDetail group = await _repository.AddAsync(albumGroup);
                    return CreatedAtRoute(
                        new { controller = "album", action = nameof(Get), key = group.Key }, group);
                }
                catch (RepositoryException e)
                {
                    return BadRequest(new ApiErrorRep(e.Message));
                }
                catch (Exception e)
                {
                    _logger.LogError("AddAlbum", e, "Error adding album");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id:int}")]
        [ProducesResponseType(typeof(AlbumGroupDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]AlbumGroup albumGroup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _repository.UpdateAsync(id, albumGroup));

                }
                catch (EntityNotFoundRepositoryException e)
                {
                    return NotFound(new ApiErrorRep(e.Message));
                }
                catch (RepositoryException e)
                {
                    return BadRequest(new ApiErrorRep(e.Message));
                }
                catch (Exception e)
                {
                    _logger.LogError("UpdateAlbumGroup", e, "Error updating album group");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id:int}/albums/add")]
        [ProducesResponseType(typeof(AlbumGroupDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAlbumToGroup([FromRoute]int id, [FromRoute]int albumId)
        {
            try
            {
                await _repository.AddAlbumAsync(id, albumId);
                return Ok();
            }
            catch (EntityNotFoundRepositoryException e)
            {
                return NotFound(new ApiErrorRep(e.Message));
            }
            catch (RepositoryException e)
            {
                return BadRequest(new ApiErrorRep(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError("AddAlbumToGroup", e, "Error adding album to group");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpPost("{id:int}/albums/remove")]
        [ProducesResponseType(typeof(AlbumGroupDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAlbumFromGroup([FromRoute]int id, [FromRoute]int albumId)
        {
            try
            {
                await _repository.RemoveAlbumAsync(id, albumId);
                return Ok();
            }
            catch (EntityNotFoundRepositoryException e)
            {
                return NotFound(new ApiErrorRep(e.Message));
            }
            catch (RepositoryException e)
            {
                return BadRequest(new ApiErrorRep(e.Message));
            }
            catch (Exception e)
            {
                _logger.LogError("RemoveAlbumFromGroup", e, "Error removing album from group");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }
    }
}