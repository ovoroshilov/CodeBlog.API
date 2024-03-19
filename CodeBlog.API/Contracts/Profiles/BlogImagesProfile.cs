using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto.BlogImgDtos;

namespace CodeBlog.API.Contracts.Profiles
{
    public class BlogImagesProfile : Profile
    {
        public BlogImagesProfile()
        {
            CreateMap<BlogImage, BlogImageDto>();
        }
    }
}
