using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAll();

        Task<Tag> Get(Guid id);

        Task<Tag> Add(Tag tag);
        
        Task<Tag?> Update(Tag tag);

        Task<Tag?> Delete(Guid id);

    }
}
