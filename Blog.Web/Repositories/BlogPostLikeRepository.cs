
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext  _dbContext;
        public BlogPostLikeRepository(BlogDbContext blogDbContext)
        {
            _dbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _dbContext.BlogPostLike.AddAsync(blogPostLike);
            await _dbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await _dbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _dbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);


        }
    }
}
