using Codexam.WebAPI.DTOs;
using Codexam.WebAPI.Entities;
using Codexam.WebAPI.Services;
using Codexam.WebAPI.Utilities.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Codexam.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenHelper _tokenHelper;
        public AuthController(IAuthService authService, ITokenHelper tokenHelper)
        {
            _authService = authService;
            _tokenHelper = tokenHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _authService.Authenticate(loginRequest);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _tokenHelper.CreateToken(user);
            return Ok(new { Token = token });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerRequest)
        {
            if (await _authService.RegisterUserAsync(registerRequest))
                return Ok("Başarılı");
            else 
                return BadRequest("Başarısız");
        }
    }
}
