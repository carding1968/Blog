using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext dbContext;
        public BlogPostRepository(BlogDbContext _blogDbContext)
        {
            dbContext = _blogDbContext;
        }
        public async Task<BlogPost> Add(BlogPost blogPost)
        {
            await dbContext.BlogPosts.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogPost?> Delete(Guid id)
        {
            var blogPost = dbContext.BlogPosts.Find(id);

            if (blogPost != null) { 
                dbContext.BlogPosts.Remove(blogPost);
                await dbContext.SaveChangesAsync();

                return blogPost;
            }

            return null;
        }

        public async Task<BlogPost?> Get(Guid id)
        {
            return await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);

            
        }

        public async Task<IEnumerable<BlogPost>> GetAll()
        {
            var blogPosts = await dbContext.BlogPosts.Include(x => x.Tags).ToListAsync();

            return blogPosts;
        }

        public async Task<BlogPost?> GetByUrlHandle(string urlHandle)
        {
            return await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);

        }

        public async Task<BlogPost?> Update(BlogPost blogPost)
        {
            var existingBlogPost = await dbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost != null) { 
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.IsVisible = blogPost.IsVisible;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.Tags = blogPost.Tags;


                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            
            }

            return null;
        }
    }
}
