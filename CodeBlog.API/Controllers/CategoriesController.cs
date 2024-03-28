using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto;
using CodeBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategotyRequestDto request)
        {
            var category = _mapper.Map<Category>(request);

            await _repository.CreateAsync(category);
            var response = _mapper.Map<CategoryResponseDto>(category);

            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _repository.GetAllAsync();
            var response = categories.Select(_mapper.Map<CategoryResponseDto>);
           
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _repository.GetById(id);
            if (category == null) return NotFound();
            return Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = id;
            category = await _repository.UpdateAsync(category);
            if (category == null) return NotFound();

            var response = _mapper.Map<CategoryResponseDto>(category);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _repository.DeleteAsync(id);
            if (category is null) return NotFound();

            var response = _mapper.Map<CategoryResponseDto>(category);

            return Ok(response);
        }
    }
}
