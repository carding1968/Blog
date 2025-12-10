namespace Blog.Web.Repositories
{
    public interface IImageRepository
    {
        Task<string> Upload(IFormFile file);
    }
}
