using Lyrics_Lab.Data;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _repository;

        public AuthController(IUserRepository repository)
        {
            _repository = repository;
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
    }
}
