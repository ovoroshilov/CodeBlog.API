using CodeBlog.API.Data;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeBlog.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;


        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IEnumerable<BlogImage>> GetAllAsync()
        {
           return await _context.BlogImages.AsNoTracking().ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var request = _httpContextAccessor.HttpContext.Request;
            var urlPath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{request.Host}{request.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";
          
            blogImage.Url = urlPath;

            await _context.BlogImages.AddAsync(blogImage);
            await _context.SaveChangesAsync();

            return blogImage;
        }
    }
}
