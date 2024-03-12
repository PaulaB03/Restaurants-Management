using backend.Models;

namespace backend.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> Login(Login user);
        Task<bool> Register(Register user);
        Task<bool> AssignRole(string email, string role);
        Task<bool> RemoveRole(string email, string role);
        Task<string> GenerateTokenString(Login user);
    }
}