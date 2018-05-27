using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Models.Image;
using MusicStoreDemo.Common.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.Api.Controllers
{
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _repo;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IImageRepository repo, ILogger<ImageController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        // GET: api/<controller>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Requesting image with ID {id}");
            var imageRef = await _repo.GetAsync(id);
            return File(imageRef.Data, imageRef.MimeType, imageRef.FileName);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromHeader(Name ="ContentType")]string contentType)
        {
            int newId = 0;
            var result = await _repo.AddAsync(new ImageReferenceDetail
            {
                Data = new byte[0],
                MimeType = contentType,
                FileName = ""
            });
            return CreatedAtRoute(
                new { controller = "image", action = "get", id = newId }, result);
        }
    }
}
