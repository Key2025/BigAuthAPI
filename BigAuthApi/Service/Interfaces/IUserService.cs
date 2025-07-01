using BigAuthApi.Model.Request;
using BigAuthApi.Model.Response;
using BigAuthApi.Models.Request;
using BigAuthApi.Models.Response;

namespace BigAuthApi.Service.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<string>> RegisterUserAsync(UserRegisterRequest req);

        Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest req);
    }
}