using Azure;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository _tagRepository, IBlogPostRepository _blogPostRepository)
        {
            tagRepository = _tagRepository;
            blogPostRepository = _blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add() {

            //GET alL TAGS
            var tags = await tagRepository.GetAll();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest) {

            BlogPost blogPost = new BlogPost();

            blogPost.ShortDescription = addBlogPostRequest.ShortDescription;
            blogPost.Author = addBlogPostRequest.Author;
            blogPost.PublishedDate = addBlogPostRequest.PublishedDate;
            blogPost.IsVisible = addBlogPostRequest.IsVisible;
            blogPost.Content = addBlogPostRequest.Content;
            blogPost.Heading = addBlogPostRequest.Heading;
            blogPost.PageTitle = addBlogPostRequest.PageTitle;
            blogPost.UrlHandle = addBlogPostRequest.UrlHandle;
            blogPost.FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl;

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags) {

                var id = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.Get(id);

                if (existingTag != null) {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            await blogPostRepository.Add(blogPost);


            return RedirectToAction("Add");
        }


        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List() {

            var blogPosts = await blogPostRepository.GetAll();

            return View(blogPosts);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {
            
            var blogPost = await blogPostRepository.Get(id);

            var tagsDomainModel = await tagRepository.GetAll();

            if (blogPost != null) {

                var editBlogPost = new EditBlogPostRequest
                {

                    Id = blogPost.Id,
                    ShortDescription = blogPost.ShortDescription,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    IsVisible = blogPost.IsVisible,
                    Content = blogPost.Content,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    UrlHandle = blogPost.UrlHandle,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Tags = tagsDomainModel.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(editBlogPost);  

            }

            return View(null);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest) {

            var edit = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                ShortDescription = editBlogPostRequest.ShortDescription,
                Author = editBlogPostRequest.Author,
                PublishedDate = editBlogPostRequest.PublishedDate,
                IsVisible = editBlogPostRequest.IsVisible,
                Content = editBlogPostRequest.Content,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                UrlHandle = editBlogPostRequest.UrlHandle,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {

                var id = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.Get(id);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            edit.Tags = selectedTags;

            var update = await blogPostRepository.Update(edit);

          


            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedBlogPost = await blogPostRepository.Delete(id);


               
             return RedirectToAction("List");
            


           
        }
    }
}
