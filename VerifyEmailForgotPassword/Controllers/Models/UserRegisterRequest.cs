using System.ComponentModel.DataAnnotations;

namespace VerifyEmailForgotPassword.Controllers.Models
{
    public class UserRegisterRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
