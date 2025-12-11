using Mini.AuthApi.Models;

namespace Mini.AuthApi.IContract
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}
