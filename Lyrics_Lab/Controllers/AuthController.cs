using Lyrics_Lab.DTOs;
using Lyrics_Lab.Helpers;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(JwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var result = _userService.Register(dto);
            return Created("Success", result);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var result = _userService.Login(dto);
            return Ok(result);
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            var result = _userService.GetUser(Request.Cookies["jwt"]!);
            return result;
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return _userService.Logout(Response);
        }

        [HttpGet("verify-session")]
        public IActionResult VerifySession()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("jwt", out var jwt))
                {
                    return Unauthorized(new { message = "No session found" });
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                return Ok(new
                {
                    isValid = true,
                    userId = userId,
                    expires = token.ValidTo
                });
            }
            catch
            {
                return Unauthorized(new { message = "Invalid session" });
            }
        }
    }
}
