using System.Security.Cryptography;
using System.Text;

namespace EInsurance_App.Helpers
{
    public class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            );
        }

        public static bool Verify(string input, string stored)
        {
            return Hash(input) == stored;
        }
    }
}
