using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TournamentsApplication.Utility
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static void ValidatePassword(string password, string passwordConfirm)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, "[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, "\\d"))
                throw new ArgumentException("Password must contain at least one digit.");

            if (!Regex.IsMatch(password, "[!@#$%^&*(),.?\\\":{ }|<>]"))
            throw new ArgumentException("Password must contain at least one special character (!@#$%^&*(),.?\":{}|<>).");

            if (Regex.IsMatch(password, "\\s"))
                throw new ArgumentException("Password must not contain whitespace.");
            if (password != passwordConfirm)
            {
                throw new Exception("Passwords dont match");
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedHash;
        }
    }
}


