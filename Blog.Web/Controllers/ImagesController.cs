using Blog.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;
        public ImagesController(IImageRepository _imageRepository)
        {
            imageRepository = _imageRepository;
        }
        /*[HttpGet]
        public IActionResult Index() {

            return Ok("This is the Get API Call");
        }*/

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) { 
            //call a repository

            var imageUrl = await imageRepository.Upload(file);

            if (imageUrl == null) {
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageUrl });

        }
    }
}
