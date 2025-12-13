using System.Diagnostics;
using Blog.Web.Models;
using Blog.Web.Models.ViewModel;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository _blogPostRepository, ITagRepository _tagRepository)
        {
            _logger = logger;
            blogPostRepository = _blogPostRepository;
            tagRepository = _tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPost = await blogPostRepository.GetAll();

            var tags = await tagRepository.GetAll();

            var model = new HomeViewModel()
            {
                BlogPosts = blogPost,
                Tags = tags


            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
