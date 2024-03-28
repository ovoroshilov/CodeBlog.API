using System.ComponentModel.DataAnnotations;

namespace CodeBlog.API.Models.Dto.AuthDtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
