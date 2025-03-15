using Lyrics_Lab.DTOs;
using Lyrics_Lab.Helpers;
using Lyrics_Lab.Models;
using Lyrics_Lab.Repositories;
using Lyrics_Lab.Repositories.Interfaces;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lyrics_Lab.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public UserService(IUserRepository repository, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public User Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _repository.Create(user);
            return user;
        }

        public IActionResult Login(LoginDto dto)
        {
            var user = _repository.GetByEmail(dto.Email);
            if (user == null)
                return new BadRequestObjectResult(new { message = "Incorrect Email" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return new BadRequestObjectResult(new { message = "Incorrect Password" });

            var jwt = _jwtService.Generate(user.Id);

            var httpContext = _httpContextAccessor.HttpContext;
            httpContext?.Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return new OkObjectResult(new { user, jwt });
        }

        public IActionResult GetUser(string jwt)
        {
            try
            {
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var user = _repository.GetById(userId);
                return new OkObjectResult(new { user, jwt });
            }
            catch
            {
                return new UnauthorizedResult();
            }
        }

        public IActionResult Logout(HttpResponse response)
        {
            response.Cookies.Delete("jwt");
            return new OkObjectResult(new { message = "Success" });
        }

        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto, ClaimsPrincipal userClaims)
        {
            var userId = userClaims.FindFirstValue("iss");

            if (userId == null)
            {
                return new UnauthorizedObjectResult(new { message = "Usuário não autenticado." });
            }

            var user = _repository.GetById(id);

            if (user == null)
            {
                return new NotFoundObjectResult("Usuário não encontrado");
            }

            if (!string.IsNullOrEmpty(updateUserDto.Name))
            {
                user.Name = updateUserDto.Name;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            }

            await _repository.UpdateUser(user);

            return new NoContentResult();
        }
    }
}
