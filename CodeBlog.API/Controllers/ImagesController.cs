using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto.BlogImgDtos;
using CodeBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public ImagesController(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var blogPosts = await _imageRepository.GetAllAsync();
            var response = blogPosts.Select(_mapper.Map<BlogImageDto>);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    CreatedDate = DateTime.Now
                };
                blogImage = await _imageRepository.Upload(file, blogImage);

                var result = _mapper.Map<BlogImageDto>(blogImage);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more then 10MB");
            }
        }
    }
}
