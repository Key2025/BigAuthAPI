using BigAuthApi.Model;

namespace BigAuthApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<int> RegisterUserAsync(string username, string email, string passwordHash, string role);

        Task<User?> FindByUsernameAsync(string username);

        Task<int> SaveRefreshTokenAsync(Guid userId, string refreshToken, DateTime refreshTokenExpiryAt);
    }
}