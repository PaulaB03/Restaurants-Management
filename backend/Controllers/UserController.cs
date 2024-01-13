using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UserController(DataContext context, IConfiguration configuration, UserService userService)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                // Validate the password
                _userService.ValidatePasswordRequirments(user.Password);

                // Check if email is unique
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return BadRequest("Email is already in use. Please choose a different email.");
                }

                // Check if username is unique
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    return BadRequest("Username is already in use. Please choose a different username.");
                }

                // Encrypt the password before storing it
                user.Password = _userService.HashPassword(user.Password);

                // Set user role
                user.Role = UserRole.user;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

        }

        // GET: api/user/id
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/user/login
        [HttpPost("login")]
        public ActionResult<User> Login(Model.Login login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);

            if (user == null || !_userService.VerifyPassword(login.Password, user.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            // Return user data without password
            user.Password = null;

            return Ok(user);
        }

    }
}
