using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogLikeRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;
        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogLikeRepository = blogPostLikeRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await _blogPostRepository.GetByUrlHandle(urlHandle);
            var blogPostLikeViewModel = new BlogDetailsViewModel();



            if (blogPost != null) {




                var totalLikes = await _blogLikeRepository.GetTotalLikes(blogPost.Id);


                if (_signInManager.IsSignedIn(User)) {

                    var likesForBlog = await _blogLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = _userManager.GetUserId(User);

                    if (userId != null) {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

                        if (likeFromUser != null)
                            liked = true;

                    }
                }

                //blog comments

                var blogCommentsDomainModel = await _blogPostCommentRepository.GetCommnetsByBlogId(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel) {
                    blogCommentsForView.Add(new BlogComment { 
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        UserName = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }


                blogPostLikeViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    IsVisible = blogPost.IsVisible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView

                };


            }

            return View(blogPostLikeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel) {

            if (_signInManager.IsSignedIn(User)) {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await _blogPostCommentRepository.Add(domainModel);

                return RedirectToAction("Index", "Blogs", new { 
                    urlHandle = blogDetailsViewModel.UrlHandle
                });
            }

            return View();

            
        }
       
    }
}
