using BigAuthApi.Model;
using BigAuthApi.Repository.Base;
using BigAuthApi.Repository.Interfaces;

namespace BigAuthApi.Repository.Main
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDapperRepositoryBase dapperBase, ILogger<UserRepository> logger) : base(dapperBase, logger)
        {
            _logger = logger;
        }

        public async Task<int> RegisterUserAsync(string username, string email, string passwordHash, string role)
        {
            var result = await ExecuteDataAsync(
                "SP_RegisterUser",
                new { Username = username, Email = email, PasswordHash = passwordHash, Role = role }
            );

            return result;
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            var result = await GetDataAsync<User>(
                "SP_GetUserByUsername",
                new { Username = username }
            );

            return result.FirstOrDefault();
        }

        public async Task<int> SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime refreshTokenExpiryAt)
        {
            var result = await ExecuteDataAsync(
                "SP_SaveRefreshToken",
                new { UserId = userId, Token = refreshToken, ExpiresAt = refreshTokenExpiryAt }
            );

            return result;
        }
    }
}