using CodeBlog.API.Models.Domain;

namespace CodeBlog.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
