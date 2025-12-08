using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext blogDbContext;

        public TagRepository(BlogDbContext _blogDbContext)
        {
            blogDbContext = _blogDbContext;
        }

        public async Task<Tag> Add(Tag tag)
        {
            await blogDbContext.Tags.AddAsync(tag);
            await blogDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> Delete(Guid id)
        {
            var tag = await blogDbContext.Tags.FindAsync(id);

            if (tag != null)
            {
                blogDbContext.Tags.Remove(tag);
                await blogDbContext.SaveChangesAsync();

                return tag;
                
            }

            return null;

        }

        public async Task<Tag> Get(Guid id)
        {
            var tag = await blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

            return tag;

        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            var tags = await blogDbContext.Tags.ToListAsync();

            return tags;
        }

        public async Task<Tag?> Update(Tag tag)
        {
            var existingTag = await blogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {

                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await blogDbContext.SaveChangesAsync();

                return existingTag;
                
            }

            return null;
        }
    }
}
