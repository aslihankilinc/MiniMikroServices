using System.Net;
using Mini.HttpServiceBus.IContract;

namespace Mini.HttpServiceBus.Services
{
    public class BaseService:IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;
    }
}
