using Mini.AuthApi.Models.Dto;
using Mini.UI.Models;

namespace Mini.UI.IContract
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool isBerarer = true);
    }
}
