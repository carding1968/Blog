using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> Add(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommnetsByBlogId(Guid blogPostId);
    }
}
