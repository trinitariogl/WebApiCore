
namespace CrossCutting.Utils.CryptoService
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Crypto
    {
        public static byte[] CreateSalt(int size)
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[size];
                provider.GetBytes(data);
                return data;
            }
        }

        public static string CreateSaltText(int size)
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[size];
                provider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// A string with hashed password.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">thrown when there's more than a user with same id satisfied by.</exception>
        public static byte[] GetHashedPassword(byte[] salt, string password)
        {
            return GetSHA256Hash(password, salt);
        }

        /* [Obsolete("Use SHA256 algoritm instead")]
         public static byte[] GetMD5Hash(string input, byte[] salt)
         {
             Guard.Against<ArgumentNullException>((salt == null) || (salt.Length == 0), Resources.SaltCannotBeNull);
             using (MD5 md = MD5.Create())
             {
                 return md.ComputeHash(Encoding.Default.GetBytes(input + salt.ToString()));
             }
         }

         [Obsolete("Use SHA256 algoritm instead")]
         public static string GetMD5HashText(string input, string salt)
         {
             StringBuilder builder = new StringBuilder();
             using (MD5 md = MD5.Create())
             {
                 byte[] buffer = md.ComputeHash(Encoding.Default.GetBytes(input + salt));
                 for (int i = 0; i < buffer.Length; i++)
                 {
                     builder.Append(buffer[i].ToString("x2", CultureInfo.InvariantCulture));
                 }
             }
             return builder.ToString();
         }*/

        public static byte[] GetSHA256Hash(string input, byte[] salt)
        {
            //Guard.Against<ArgumentNullException>((salt == null) || (salt.Length == 0), Resources.SaltCannotBeNull);
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.Default.GetBytes(input + salt.ToString()));
            }
        }

        /*public static string GetSHA256HashText(string input, string salt)
        {
            StringBuilder builder = new StringBuilder();
            using (SHA256 sha = SHA256.Create())
            {
                byte[] buffer = sha.ComputeHash(Encoding.Default.GetBytes(input + salt));
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x2", CultureInfo.InvariantCulture));
                }
            }
            return builder.ToString();
        }*/
    }
}
