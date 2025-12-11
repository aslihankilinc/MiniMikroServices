using Microsoft.AspNetCore.Identity;

namespace Mini.AuthApi.Models
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
    }
}
