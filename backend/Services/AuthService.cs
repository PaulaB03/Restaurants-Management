using backend.Data;
using backend.Models;
using backend.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _config = config;
        }

        public async Task<bool> Login(Login user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public async Task<bool> Register(Register user)
        {
            // Create address
            var address = new Address
            {
                Street = user.Address.Street,
                Floor = user.Address.Floor,
                Number = user.Address.Number,
                City = user.Address.City
            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            // Create user
            var identityUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            // Create role "User"
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(identityUser, "User");
            }

            return result.Succeeded;
        }

        public async Task<bool> AssignRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(role); 
            if (!roleExists)
            {
                return false; // Invalid role
            }

            if (await _userManager.IsInRoleAsync(user, role))
            {
                return true; // User already in role
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                return false; // Invalid role
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        public async Task<bool> IsUsernameUnique(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user == null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.PasswordHash = null;
            return user;
        }

        public async Task<string> GenerateTokenString(Login user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            if (identityUser == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Add a claim for each role the user has
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
