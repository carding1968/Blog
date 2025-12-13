using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAll();
        Task<BlogPost?> Get(Guid id);

        Task<BlogPost> Add(BlogPost blogPost);

        Task<BlogPost?> Update(BlogPost blogPost);

        Task<BlogPost?> Delete(Guid id);

        Task<BlogPost?> GetByUrlHandle(string urlHandle);

    }
}
