using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        
        private readonly ITagRepository tagRepository;
        public AdminTagsController(ITagRepository _tagRepository)
        {
            tagRepository = _tagRepository;
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
            
            await tagRepository.Add(tag);
            
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List() {
           
           var tags = await tagRepository.GetAll(); 



            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {

            //var tag = _blogDbContext.Tags.Find(id);

            var tag = await tagRepository.Get(id);


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

            var updateTag = await tagRepository.Update(tag);

/*
            if (updateTag != null)
            {

            }
            else { 
            
            }*/


            return RedirectToAction("Edit", new { id = editTagRequest.Id });
                

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) {
            var tag = await tagRepository.Delete(editTagRequest.Id);

            if (tag != null) {
                

                //show success notif

                return RedirectToAction("List");
            }


            //show error notif
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

    }
}
