using Blog.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        public BlogsController(IBlogPostRepository _blogPostRepository)
        {
            blogPostRepository = _blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {

            var url = await blogPostRepository.GetByUrlHandle(urlHandle);
            return View(url);
        }
    }
}
