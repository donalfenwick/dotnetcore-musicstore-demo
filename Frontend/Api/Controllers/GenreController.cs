using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreDemo.Common.Models;
using MusicStoreDemo.Database;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Common.Models.Genre;
using MusicStoreDemo.Database.Entities;
using System.Net;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Dynamic;
using MusicStoreDemo.Common.Models.Enum;

namespace MusicStoreDemo.Api.Controllers
{
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _repo;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreRepository repo, ILogger<GenreController> logger){
            _repo = repo;
            _logger = logger;
        }


        [HttpGet("")]
        //[Authorize(Policy = "musicStoreApi.readAccess")]
        [ProducesResponseType(typeof(GenreList),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> List()
        {
            try{
                GenreList result = await _repo.ListAsync(contentPublicationFlags: PublishStatus.PUBLISHED);
                return Ok(result);
            }catch(Exception e){
                _logger.LogError("AddArtist", e, "Error adding artist");
                return StatusCode(500, new ApiErrorRep("Unknown error"));
            }
        }

        [HttpGet("{genre}")] 
        //[Authorize(Policy = "musicStoreApi.readAccess")]
        [ProducesResponseType(typeof(GenreDetail),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute]string genre = null)
        {
            if (!string.IsNullOrWhiteSpace(genre))
            {
                try{
                    GenreDetail genreResult = await _repo.GetAsync(genre, contentPublicationFlags: PublishStatus.PUBLISHED);
                    if(genreResult != null){
                        return Ok(genreResult);
                    }else{
                        return NotFound(new ApiErrorRep($"Genre not found"));
                    }
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("AddArtist", e, "Error adding artist");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }
            else{
                return BadRequest(new ApiErrorRep("No genre provided"));
            }
        }

        [HttpPut("{genre}")]
        [Authorize(Policy = "musicStoreApi.writeAccess")]
        [ProducesResponseType(typeof(GenreDetail),(int)HttpStatusCode.Created)]
        public async Task<IActionResult> Put([FromRoute]string genre = null)
        {
            if (!string.IsNullOrWhiteSpace(genre))
            {
                try{
                    GenreDetail existingGenre = await _repo.GetAsync(genre, contentPublicationFlags: PublishStatus.PUBLISHED);
                    if(existingGenre!= null){
                        return Ok(existingGenre);
                    }else{
                        GenreDetail result = await _repo.AddAsync(genre);
                        return this.CreatedAtRoute(new { controller = "genre", action = nameof(Get), genre = genre }, result);
                    }
                }catch(RepositoryException e){
                    return BadRequest(new ApiErrorRep(e.Message));
                }catch(Exception e){
                    _logger.LogError("AddArtist", e, "Error adding artist");
                    return StatusCode(500, new ApiErrorRep("Unknown error"));
                }
            }
            else
            {
                return BadRequest(new ApiErrorRep("No genre provided"));
            }
        }
    }
}
