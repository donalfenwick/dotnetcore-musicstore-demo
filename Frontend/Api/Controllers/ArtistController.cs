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
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Api.Controllers
{
    [Route("api/artist")]
    public class ArtistController : Controller
    {
        private readonly ArtistMapper _artistMapper;
        private readonly IArtistRepository _repo;
        private readonly ILogger<ArtistController> _logger;
        public ArtistController(IArtistRepository repo, ILogger<ArtistController> logger)
        {
            _artistMapper = new ArtistMapper();
            _repo = repo;
            _logger = logger;
        }

        [HttpGet(""), Produces(typeof(ArtistList))]
        [ProducesResponseType(typeof(ArtistList), 200)]
        [ProducesResponseType(typeof(ApiErrorRep), 400)]
        public async Task<IActionResult> List([FromQuery]int page = 0, [FromQuery]int pageSize = 20)
        {
            try{
                ArtistList result = await _repo.ListAsync(page, pageSize, PublishStatus.PUBLISHED);
                return Ok(result);
            }catch(EntityNotFoundRepositoryException e){
                return NotFound(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("ListArtist", e, "Error listing artists");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        // GET api/values/5
        [HttpGet("{id}"),ProducesResponseType(typeof(ArtistDetail), 200)]
        public async Task<IActionResult> Get(int id)
        {
            try{
                ArtistDetail artist = await _repo.GetAsync(id);
                if(artist != null){
                    return Ok(artist);
                }else{
                    return NotFound(new ApiErrorRep($"Artist with ID {id} was nto found"));
                }
            }catch(RepositoryException e){
                return BadRequest(new ApiErrorRep(e.Message));
            }catch(Exception e){
                _logger.LogError("GetArtist", e, "Error getting artist");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(ArtistDetail), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiErrorRep), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody]Artist artist)
        {
            if(ModelState.IsValid){
                try{
                    ArtistDetail newArtist = await _repo.AddAsync(artist);
                    return CreatedAtRoute(
                        new { controller = "artist", action = nameof(Get), id = newArtist.Id }
                        , newArtist);
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("AddArtist", e, "Error adding artist");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }else{
                return BadRequest(ModelState);
            }
        }

        [HttpPost("{id:int}")]
        [ProducesResponseType(typeof(ArtistDetail), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorRep),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]Artist artist) 
        {
            if(ModelState.IsValid){
                try{
                    return Ok(await _repo.UpdateAsync(id, artist));

                }catch(EntityNotFoundRepositoryException e){
                    return NotFound(new ApiErrorRep(e.Message));
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("UpdateArtist", e, "Error updateing artist");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }else{
                return BadRequest(ModelState);
            }
        }
    }
}
