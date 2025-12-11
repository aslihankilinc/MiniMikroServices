using Microsoft.AspNetCore.Identity;
using Mini.AuthApi.Data;
using Mini.AuthApi.IContract;
using Mini.AuthApi.Models;
using Mini.AuthApi.Models.Dto;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Mini.AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private ILogger<AuthService> _logger;
        private readonly UserManager<User> _userManager;
        private AppDbContext db;
        private readonly IJwtService _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRabbitMQAuthMessageSender _rabbit;
        public AuthService(ILogger<AuthService> logger,
            UserManager<User> userManager,
            AppDbContext context,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwt,
            IRabbitMQAuthMessageSender rabbit)
        {
            _logger = logger;
            _userManager = userManager;
            db = context;
            _roleManager = roleManager;
            _jwt = jwt;
            _rabbit = rabbit;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == request.UserName);
            bool isPass = await _userManager.CheckPasswordAsync(user, request.Password);
            if (user is null || !isPass)
                return new LoginResponseDto() { User = null, Token = "" };

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwt.GenerateToken(user, roles.ToList());

            UserDto userDTO = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };
            _rabbit.SendMessage($"Kullanıcı Girişi {JsonConvert.SerializeObject(userDTO)}", "AuthLoginQueue");

            return loginResponseDto;
        }

        public async Task<ResponseDto> Register(RegisterationRequestDto request)
        {
            var user = new User()
            {
                UserName = request.Email,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };
            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var userToReturn = db.Users.FirstOrDefault(u => u.Email == request.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    AssignRole(userDto.Id, request.Role);
                    return new ResponseDto { IsSuccess = true };
                }
                else
                {
                    return new ResponseDto { IsSuccess = false, Message = result.Errors.FirstOrDefault().Description };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = ex.ToString() };
            }

        }

        public async Task<bool> AssignRole(string id, string roleName)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                var roleManager = await _roleManager.RoleExistsAsync(roleName);
                if (!roleManager)
                {
                    var userRole = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

    }
}
