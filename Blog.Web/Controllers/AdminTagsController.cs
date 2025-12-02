using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext blogDbContext;
        public AdminTagsController(BlogDbContext _blogDbContext)
        {
            blogDbContext = _blogDbContext;            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest) 
        {
            Tag tag = new Tag();
            tag.Name = addTagRequest.Name;
            tag.DisplayName = addTagRequest.DisplayName;
            
            await blogDbContext.Tags.AddAsync(tag);
            await blogDbContext.SaveChangesAsync();
            
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List() {
            var tags = await blogDbContext.Tags.ToListAsync();



            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {

            //var tag = _blogDbContext.Tags.Find(id);

            var tag = await blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);


            if (tag != null) {

                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,

                };
                return View(editTagRequest);
            }

            
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest) {

            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            var existingTag = await blogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null) { 
               
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                
                
                await blogDbContext.SaveChangesAsync();

                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
                

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) {
            var tag = await blogDbContext.Tags.FindAsync(editTagRequest.Id);

            if (tag != null) {
                blogDbContext.Tags.Remove(tag);
                await blogDbContext.SaveChangesAsync();


                //show success notif

                return RedirectToAction("List");
            }


            //show error notif
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}
