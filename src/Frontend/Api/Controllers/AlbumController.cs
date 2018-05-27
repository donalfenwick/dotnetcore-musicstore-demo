using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreDemo.Common.Models.Artist;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using System.Net;
using MusicStoreDemo.Common.Models;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common.Repositories;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Models.Album;
using Microsoft.AspNetCore.Authorization;
using MusicStoreDemo.Common.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Common.Models.Album;
using Microsoft.Extensions.Configuration;

namespace MusicStoreDemo.Api.Controllers
{
    [Route("api/album")]
    public class AlbumController : Controller
    {
        private readonly IAlbumRepository _repository;
        private readonly ILogger<AlbumController> _logger;
        private readonly UserManager<DbUser> _userManager;
        private readonly IConfiguration _configuration;

        public AlbumController(IAlbumRepository repository, ILogger<AlbumController> logger, UserManager<DbUser> userManager,  IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpGet("")]
        public async Task<IActionResult> List([FromQuery]int page = 0, [FromQuery]int pageSize = 20){
            string databaseProvider =  _configuration.GetValue<string>("MusicStoreAppDatabaseProvider");
            for(int i = 0; i < 20; i++){_logger.LogDebug("MusicStoreAppDatabaseProvider"+(databaseProvider??"null"));}
            try{
                var result = await _repository.ListAsync(page, pageSize, PublishStatus.PUBLISHED);
                return Ok(result);
            }catch(EntityNotFoundRepositoryException e){
                return NotFound(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("ListAlbum", e, "Error listing albums");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string query, [FromQuery]int page = 0, [FromQuery]int pageSize = 20){
                try{
                var result = await _repository.SearchAsync(query, page, pageSize, PublishStatus.PUBLISHED);
                return Ok(result);
            }catch(EntityNotFoundRepositoryException e){
                return NotFound(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("ListAlbum", e, "Error listing albums");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpGet("/api/artist/{id}/albums")]
        public async Task<IActionResult> GetAlbumsByArtist([FromRoute]int id, [FromQuery]int page = 0, [FromQuery]int pageSize = 20){
                try{
                var result = await _repository.ListByArtistAsync(id, page, pageSize, PublishStatus.PUBLISHED);
                return Ok(result);
            }catch(EntityNotFoundRepositoryException e){
                return NotFound(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("ListAlbum", e, "Error listing albums");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpGet("group/{key}")]
        public async Task<IActionResult> ListGroup([FromRoute]string key, [FromQuery]int page = 0, [FromQuery]int pageSize = 20)
        {
            try
            {
                AlbumList result = await _repository.ListByAlbumGroupKeyAsync(key, page, pageSize, PublishStatus.PUBLISHED);
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

        [Authorize]
        [HttpGet("purchased")]
        public async Task<IActionResult> GetAlbumsForUser([FromQuery]int page = 0, [FromQuery]int pageSize = 20)
        {
            try
            {
                DbUser usr = await this._userManager.GetUserAsync(this.User);
                if (usr != null)
                {
                    AlbumList result = await _repository.ListUserPurchasedAlbumsAsync(usr.Id, page, pageSize, PublishStatus.PUBLISHED);
                    return Ok(result);
                }else{
                    return Unauthorized();
                }
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArtistDetail), 200)]
        public async Task<IActionResult> Get(int id)
        {
            try{
                AlbumDetail a = await _repository.GetAsync(id);
                if(a != null){
                    return Ok(a);
                }else{
                    return NotFound(new ApiErrorRep($"Album with ID {id} was nto found"));
                }
            }catch(RepositoryException e){
                return BadRequest(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("GetArtist", e, "Error getting album");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [Authorize]
        [HttpGet("{id}/owenershipstatus")]
        [ProducesResponseType(typeof(UserAlbumOwnershipStatus), 200)]
        public async Task<IActionResult> GetOwnershipStatus(int id)
        {
            try{

                DbUser usr = await this._userManager.GetUserAsync(this.User);
                if (usr != null)
                {
                    UserAlbumOwnershipStatus status = await _repository.GetAlbumOwnershipStatusForUser(id, usr.Id, PublishStatus.PUBLISHED);
                    if(status != null){
                        return Ok(status);
                    }else{
                        return NotFound(new ApiErrorRep($"Album with ID {id} was nto found"));
                    }
                }else{
                    return Unauthorized();
                }
            }catch(RepositoryException e){
                return BadRequest(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("GetArtist", e, "Error getting album");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(ArtistDetail), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody]Album album)
        {
            if(ModelState.IsValid){
                try{
                    AlbumDetail a = await _repository.AddAsync(album);
                    return CreatedAtRoute(
                        new { controller = "album", action = nameof(Get), id = a.Id }
                        , a);
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("AddAlbum", e, "Error adding album");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }else{
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id:int}")]
        [ProducesResponseType(typeof(ArtistDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorRep),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]Album album) 
        {
            if(ModelState.IsValid){
                try{
                    return Ok(await _repository.UpdateAsync(id, album));

                }catch(EntityNotFoundRepositoryException e){
                    return NotFound(new ApiErrorRep(e.Message));
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("UpdateAlbum", e, "Error updateing album");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }else{
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpPost("{id:int}/purchase")]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Purchase([FromRoute]int id)
        {
            DbUser usr = await this._userManager.GetUserAsync(this.User);
            if (usr != null)
            {
                try
                {
                    await _repository.PurchaseAsync(id, usr.Id);
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
                    _logger.LogError("UpdateAlbum", e, "Error updateing album");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}