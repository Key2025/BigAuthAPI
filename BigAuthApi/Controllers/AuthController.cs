using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Models.Request;
using BigAuthApi.Models.Response;
using BigAuthApi.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse<string>>> Register([FromBody] UserRegisterRequest req)
        {
            var result = await _userService.RegisterUserAsync(req);
            return result;
        }

        [HttpPost("login")]
        public async Task<BaseResponse<LoginResponse>> Login([FromBody] LoginRequest req)
        {
            var result = await _userService.LoginAsync(req);
            return result;
        }

        [Authorize(Roles = "USER,ADMIN")]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (User.IsInRole("ADMIN"))
                return Ok("WELCOME ADMIN");

            if (User.IsInRole("USER"))
                return Ok("WELCOME USER");

            return Unauthorized("Unknown role");
        }
    }
}