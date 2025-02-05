using Lyrics_Lab.DTOs;
using Lyrics_Lab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lyrics_Lab.Services.Interfaces
{
    public interface IUserService
    {
        User Register(RegisterDto dto);
        IActionResult Login(LoginDto dto);
        IActionResult GetUser(string jwt);
        IActionResult Logout(HttpResponse response);
        Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto, ClaimsPrincipal userClaims);
    }
}
