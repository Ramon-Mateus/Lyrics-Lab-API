using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lyrics_Lab.Services.Interfaces
{
    public interface IUserService
    {
        User Register(RegisterDto dto);
        IActionResult Login(LoginDto dto);
        IActionResult GetUser(string jwt);
        IActionResult Logout(HttpResponse response);
    }
}
