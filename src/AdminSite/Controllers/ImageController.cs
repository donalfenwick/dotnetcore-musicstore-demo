using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Common.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.AdminSite.Controllers
{
	[Route("image")]
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
            var imageRef = await _repo.GetAsync(id);
            return File(imageRef.Data, imageRef.MimeType, imageRef.FileName);
        }
    }
}
