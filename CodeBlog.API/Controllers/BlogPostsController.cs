using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto;
using CodeBlog.API.Models.Dto.BlopPostDtos;
using CodeBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequest request)
        {
            var blogPost = _mapper.Map<BlogPost>(request);

            foreach(var categoryId in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryId);

                if (existingCategory is not null) {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await _blogPostRepository.CreateAsync(blogPost);

            var response = _mapper.Map<BlogPostDto>(blogPost);
            response.Categories = blogPost.Categories.Select(_mapper.Map<CategoryResponseDto>).ToList();

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAlBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();
            var response = blogPosts.Select(_mapper.Map<BlogPostDto>);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAlBlogPostById([FromRoute]Guid id)
        {
            var blogPost = await _blogPostRepository.GetById(id);
            if (blogPost is null) return NotFound();

            var response = _mapper.Map<BlogPostDto>(blogPost);

            return Ok(response);
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandle(urlHandle);
            if (blogPost is null) return NotFound();

            var response = _mapper.Map<BlogPostDto>(blogPost);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, [FromBody]UpdateBlogPostRequestDto request)
        {
            var blogPost = _mapper.Map<BlogPost>(request);
            blogPost.Id = id;   

            foreach (var categoryId in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetById(categoryId);

                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var updatedPost = await _blogPostRepository.UpdateAsync(blogPost);

            if(updatedPost is null) return NotFound();

            var response = _mapper.Map<BlogPostDto>(updatedPost);
            response.Categories = blogPost.Categories.Select(_mapper.Map<CategoryResponseDto>).ToList();

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.DeleteAsync(id);
            if (blogPost is null) return NotFound();

            var response = _mapper.Map<BlogPostDto>(blogPost);

            return Ok(response);
        }
    }
}
