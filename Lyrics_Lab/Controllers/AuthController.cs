using Lyrics_Lab.Contexts;
using Lyrics_Lab.DTOs;
using Lyrics_Lab.Helpers;
using Lyrics_Lab.Models;
using Lyrics_Lab.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lyrics_Lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        //private readonly JwtService _jwtService;
        //private readonly IUserService _userService;

        //public AuthController(JwtService jwtService, IUserService userService)
        //{
        //    _jwtService = jwtService;
        //    _userService = userService;
        //}

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Usuário não encontrado");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return BadRequest("Senha incorreta");

            return Ok(new
            {
                Success = true,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model, [FromServices] ApplicationDbContext context)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var album = new Album
            {
                Name = "Default",
                Description = "Default Album",
                IsDefault = true,
                UserId = user.Id
            };

            context.Albums.Add(album);
            await context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new
            {
                Success = true,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Success = true, Message = "Logout realizado com sucesso" });
        }

        //[HttpPost("register")]
        //public IActionResult Register(RegisterDto dto)
        //{
        //    var result = _userService.Register(dto);
        //    return Created("Success", result);
        //}

        //[HttpPost("login")]
        //public IActionResult Login(LoginDto dto)
        //{
        //    var result = _userService.Login(dto);
        //    return Ok(result);
        //}

        //[HttpGet("user")]
        //public IActionResult GetUser()
        //{
        //    var result = _userService.GetUser(Request.Cookies["jwt"]!);
        //    return result;
        //}

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    return _userService.Logout(Response);
        //}
    }
}
