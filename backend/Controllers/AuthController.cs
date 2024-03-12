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
    }
}
