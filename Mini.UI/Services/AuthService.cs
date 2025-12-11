using Microsoft.AspNetCore.Identity.Data;
using Mini.AuthApi.Models.Dto;
using Mini.UI.IContract;
using Mini.UI.Models;
using Mini.UI.Models.Dto;
using Mini.UI.Utility;

namespace Mini.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = request,
                Url = StaticBase.AuthApiBase + "/api/auth/Login"
            });

        }

        public async  Task<ResponseDto> Register(RegisterRequest request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = EnumApiType.POST,
                Data = request,
                Url = StaticBase.AuthApiBase + "/api/auth/Register"
            });
        }
    }
}
