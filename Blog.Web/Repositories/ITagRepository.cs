using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageNumber = 1, int pageSize = 100);

        Task<Tag> Get(Guid id);

        Task<Tag> Add(Tag tag);
        
        Task<Tag?> Update(Tag tag);

        Task<Tag?> Delete(Guid id);

        Task<int> Count();

    }
}
