using System.Security.Cryptography;
using System.Text;

//Класс, отвечающий за хэширование паролей

namespace Blog
{
    public class PasswordHash
    {
        /// <summary>
        /// Метод для хэширования пролей
        /// </summary>
        /// <param name="enteredPassword">Пароль</param>
        /// <returns>Hashed parolString</returns>
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
