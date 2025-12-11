using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini.AuthApi.IContract;
using Mini.AuthApi.Models.Dto;

namespace Mini.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private  ResponseDto response;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            response = new ResponseDto();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            
           LoginResponseDto login = await _authService.Login(request);
            if (login.User is null)
            {
                response.Message = "Username or password is incorrect";
                return BadRequest(response);
            }
            response.Result = login;
            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterationRequestDto request)
        {
            response=await _authService.Register(request);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
