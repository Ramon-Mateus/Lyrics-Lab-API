using BCrypt.Net;
using Lyrics_Lab.Data;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Helpers;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            return Created("Success", _repository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);

            if (user == null) return BadRequest(new { message = "Incorrect Email" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password)) return BadRequest(new { message = "Incorrect Password" });

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new
            {
                user,
                jwt
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);

                return Ok(new {
                        user,
                        jwt
                });
            }
            catch (Exception _)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "Success"
            });
        }
    }
}
