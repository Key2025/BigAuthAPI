using BigAuthApi.Model;
using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Models.Request;
using BigAuthApi.Models.Response;
using BigAuthApi.Repository.Interfaces;
using BigAuthApi.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BigAuthApi.Service
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(IConfiguration config, ILogger<UserService> logger, IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher, IJwtService jwtService)
        {
            _config = config;
            _logger = logger;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<BaseResponse<string>> RegisterUserAsync(UserRegisterRequest req)
        {
            // Runtime model validation
            var context = new ValidationContext(req);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(req, context, results, true);

            if (!isValid)
            {
                var message = string.Join("; ", results.Select(r => r.ErrorMessage));
                return BaseResponse<string>.Fail(message);
            }

            try
            {
                var username = req.Username.Trim().ToUpper();
                var email = req.Email.Trim().ToLower();
                var passwordHash = _passwordHasher.HashPassword(new User(), req.Password);
                var role = req.Role.Trim().ToUpper();

                var rows = await _userRepository.RegisterUserAsync(username, email, passwordHash, role);
                if (rows == 1)
                    return BaseResponse<string>.Ok("Registration successful");
                else
                    return BaseResponse<string>.Fail("No rows affected");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("ALREADY_EXISTS"))
                {
                    return BaseResponse<string>.Fail("Username or email already exists");
                }
                else
                {
                    _logger.LogError(ex, "Unexpected error during registration");
                    return BaseResponse<string>.Fail("Internal server error");
                }
            }
        }

        public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest req)
        {
            // Runtime model validation
            var context = new ValidationContext(req);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(req, context, results, true);

            if (!isValid)
            {
                var message = string.Join("; ", results.Select(r => r.ErrorMessage));
                return BaseResponse<LoginResponse>.Fail(message);
            }
            try
            {
                var user = await _userRepository.FindByUsernameAsync(req.Username);
                if (user == null || !user.IsActive)
                    return BaseResponse<LoginResponse>.Fail("User not found");

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
                if (result != PasswordVerificationResult.Success)
                    return BaseResponse<LoginResponse>.Fail("Username and Password is invalid");

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var refreshTokenExpiryAt = DateTime.UtcNow.AddDays(int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"]!));

                await _userRepository.SaveRefreshTokenAsync(user.Id, refreshToken, refreshTokenExpiryAt);

                return BaseResponse<LoginResponse>.Ok(new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(30)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration");
                return BaseResponse<LoginResponse>.Fail("Internal server error");
            }
        }
    }
}