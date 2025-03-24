using Codexam.WebAPI.DTOs;
using Codexam.WebAPI.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace Codexam.WebAPI.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterUserAsync(RegisterDto registerRequest);
    }
}