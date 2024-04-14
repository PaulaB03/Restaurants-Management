using backend.Models;
using backend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Register user)
        {
            if (await _authService.Register(user))
            {
                return Ok("User created");
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login user)
        {
            if (await _authService.Login(user))
            {
                var tokenString = await _authService.GenerateTokenString(user);
                return Ok(tokenString);
            }

            return BadRequest();
        }

        [HttpGet("CheckEmail")]
        public async Task<IActionResult> CheckEmailUnique(string email)
        {
            bool isUnique = await _authService.IsEmailUnique(email);
            return Ok(isUnique);
        }

        [HttpGet("CheckUsername")]
        public async Task<IActionResult> CheckUsernameUnique(string username)
        {
            bool isUnique = await _authService.IsUsernameUnique(username);
            return Ok(isUnique);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _authService.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found.");
        }
    }
}
