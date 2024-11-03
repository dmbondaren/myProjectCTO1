using System.Security.Cryptography;
using System.Text;

namespace myProjectCTO
{
    internal static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password); // Измените на UTF8
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2")); // Конвертуємо у шістнадцятковий формат
                }

                return sb.ToString();
            }
        }
    }
}
