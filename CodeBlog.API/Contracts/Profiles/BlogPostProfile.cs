using AutoMapper;
using CodeBlog.API.Models.Domain;
using CodeBlog.API.Models.Dto.BlopPostDtos;

namespace CodeBlog.API.Contracts.Profiles
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<CreateBlogPostRequest, BlogPost>().ForMember(x => x.Categories, opt => opt.Ignore());
            CreateMap<BlogPost, BlogPostDto>(); 
            CreateMap<UpdateBlogPostRequestDto, BlogPost>().ForMember(x => x.Categories, opt => opt.Ignore());

        }
    }
}
