using System.Security.Cryptography;
using System.Text;

//Класс, отвечающий за хэширование паролей

namespace Blog
{
    public class PasswordHash
    {
        public static string HashPassword(string enteredPassword)
        {
            using(var HashP = SHA256.Create())
            {
              var hashedBytes = HashP.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
              var hash = BitConverter.ToString(hashedBytes).Replace("-","").ToLower();
              return hash;
            }
        }
    }
}
