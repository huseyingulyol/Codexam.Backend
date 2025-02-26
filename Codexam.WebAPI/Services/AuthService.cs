using Azure.Core;
using Codexam.WebAPI.DTOs;
using Codexam.WebAPI.Entities;
using Codexam.WebAPI.Repositories;
using Codexam.WebAPI.Utilities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Codexam.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(RegisterDto registerRequest)
        {
            // Mail var mı kontrol et.
            var existingUser = await _userRepository.GetUserByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Kullanıcı zaten mevcut.");
            }

            //Yoksa Oluştur
            User newUser = new();

            byte[] passwordSalt, passwordHash;

            HashingHelper.CreatePasswordHash(registerRequest.Password, out passwordSalt, out passwordHash);
            newUser.PasswordSalt = passwordSalt;
            newUser.PasswordHash = passwordHash;
            newUser.RoleId = 1;

            await _userRepository.AddUserAsync(newUser);

            return true;
        }

        public async Task<User> Authenticate(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByCredentialsAsync(loginRequest.Email, loginRequest.Password);
            return user;
        }

    }
}
