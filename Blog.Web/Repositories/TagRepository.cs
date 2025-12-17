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

        public async Task<int> Count()
        {
            return await blogDbContext.Tags.CountAsync();
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

        public async Task<IEnumerable<Tag>> GetAll(string? searchQuery, string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100)
        {
            //var tags = await blogDbContext.Tags.Where(x => x %).ToListAsync();

            var query = blogDbContext.Tags.AsQueryable();

            if (string.IsNullOrWhiteSpace(searchQuery) == false) {
                query = query.Where(x => x.Name.Contains(searchQuery) || x.DisplayName.Contains(searchQuery));
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDec = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase)) {
                    query = isDec ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDec ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }


            }

            //pagination
            var skipResult = (pageNumber - 1) * pageSize;

            query = query.Skip(skipResult).Take(pageSize);




            return await query.ToListAsync();
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
