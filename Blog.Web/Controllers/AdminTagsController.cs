using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext _blogDbContext;
        public AdminTagsController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest) 
        {
            Tag tag = new Tag();
            tag.Name = addTagRequest.Name;
            tag.DisplayName = addTagRequest.DisplayName;
            
            _blogDbContext.Tags.Add(tag);
            _blogDbContext.SaveChanges();
            return View("Add");
        }

        [HttpGet]
        public IActionResult List() {
            var tags = _blogDbContext.Tags.ToList();



            return View(tags);
        }

    }
}
