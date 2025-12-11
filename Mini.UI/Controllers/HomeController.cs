using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Mini.AuthApi.Models.Dto;
using Mini.UI.IContract;
using Mini.UI.Models;
using Mini.UI.Models.Dto;
using Mini.UI.Services;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Mini.UI.Controllers
{
    public class HomeController(ILogger<HomeController> _logger, IAuthService _auth, ITokenService _token) : Controller
    {
        


        public IActionResult Index()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _auth.Login(obj);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                SigInUser(loginResponseDto);
                _token.SetToken(loginResponseDto.Token);
                return RedirectToAction("Privacy", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //cookie tabanlı oturum açmasılması
        private async Task SigInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            //Token decode ediliyor
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            //Roller ekleniyor
            identity.AddClaim(new Claim(ClaimTypes.Name,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            //Kullanıcı “principal” oluşturuluyor
            //ClaimsPrincipal, kimlik bilgilerini (identity) ve claim’leri taşıyan nesnedir
            var principal = new ClaimsPrincipal(identity);
            //Oturum açma işlemi gerçekleştiriliyor
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
