using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {

        private readonly BlogDbContext _dbContext;

        public BlogPostCommentRepository(BlogDbContext blogDbContext)
        {
            _dbContext = blogDbContext;
        }

        public async Task<BlogPostComment> Add(BlogPostComment blogPostComment)
        {
            await _dbContext.BlogPostComment.AddAsync(blogPostComment);
            await _dbContext.SaveChangesAsync();

            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommnetsByBlogId(Guid blogPostId)
        {
            return await _dbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
            
        }
    }
}
