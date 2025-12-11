using Microsoft.AspNetCore.Identity.Data;
using Mini.AuthApi.Models.Dto;
using Mini.UI.Models.Dto;

namespace Mini.UI.IContract
{
    public interface IAuthService
    {
         Task<LoginResponseDto> Login(LoginRequestDto requestD);
         Task<ResponseDto> Register(RegisterRequest register);
    }
}
