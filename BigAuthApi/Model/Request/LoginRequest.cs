using System.ComponentModel.DataAnnotations;

namespace BigAuthApi.Models.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}