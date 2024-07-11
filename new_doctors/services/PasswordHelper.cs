using System.Security.Cryptography;
using System.Text;

public class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();
            foreach (var b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        string hashOfEnteredPassword = HashPassword(enteredPassword);
        return hashOfEnteredPassword == storedHash;
    }
}
