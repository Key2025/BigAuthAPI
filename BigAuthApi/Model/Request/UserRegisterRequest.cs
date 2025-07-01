using BigAuthApi.Constants;
using System.ComponentModel.DataAnnotations;

namespace BigAuthApi.Model.Request
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = BigAuthRole.User;
    }
}