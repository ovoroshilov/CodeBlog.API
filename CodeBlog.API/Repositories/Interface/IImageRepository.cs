using CodeBlog.API.Models.Domain;
using System.Collections.Generic;

namespace CodeBlog.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
        Task<IEnumerable<BlogImage>> GetAllAsync();
    }
}
