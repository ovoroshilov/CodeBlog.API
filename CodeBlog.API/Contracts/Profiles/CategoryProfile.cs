using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto;

namespace CodeBlog.API.Contracts.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategotyRequestDto, Category>();
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<UpdateCategoryRequestDto, Category>();
        }
    }
}
