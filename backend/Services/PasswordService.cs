namespace backend.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public void ValidatePasswordRequirments(string password)
        {
            if (password.Length < 10)
            {
                throw new ArgumentException("Password must be at least 10 characters long");
            } 
            else if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("Password must contain at least a digit");
            }
            else if (!password.Any(char.IsUpper))
            {
                throw new ArgumentException("Password must contain at least an upper letter");
            }
            else if (!password.Any(char.IsLower))
            {
                throw new ArgumentException("Password must ocontain at least an lower letter");
            }
        }
    }
}
