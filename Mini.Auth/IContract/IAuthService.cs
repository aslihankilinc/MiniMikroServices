using Mini.AuthApi.Models.Dto;

namespace Mini.AuthApi.IContract
{
    public interface IAuthService
    {
        public Task<ResponseDto> Register(RegisterationRequestDto request);

        public Task<LoginResponseDto> Login(LoginRequestDto request);

    }
}
