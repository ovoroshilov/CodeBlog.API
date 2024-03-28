using CodeBlog.API.Models.Dto.AuthDtos;
using CodeBlog.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenRepository _jwtTokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, IJwtTokenRepository jwtTokenRepository)
        {
            _userManager = userManager;
            _jwtTokenRepository = jwtTokenRepository;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
           var identityUser = await _userManager.FindByEmailAsync(request.Email);
            if(identityUser is not null)
            {
               var checkPasswordRes = await _userManager.CheckPasswordAsync(identityUser, request.Password);
                if (checkPasswordRes)
                {
                    var roles = await _userManager.GetRolesAsync(identityUser);
                    var response = new LoginResponseDto
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = _jwtTokenRepository.CreateToken(identityUser, roles.ToList())
                    };
                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Wrong email or password");

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
            };
            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
               identityResult = await _userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}
