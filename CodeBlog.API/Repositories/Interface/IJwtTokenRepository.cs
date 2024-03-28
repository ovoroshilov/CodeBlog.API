using Microsoft.AspNetCore.Identity;

namespace CodeBlog.API.Repositories.Interface
{
    public interface IJwtTokenRepository
    {
        string CreateToken(IdentityUser user, List<string> roles);
    }
}
