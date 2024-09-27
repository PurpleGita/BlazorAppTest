using System.Security.Cryptography;

namespace BlazorAppTest.Hashing
{
    public class HashingHandler
    {
        
        public string someMash(string textToHash) 
        {
            return (textToHash);
        }


        public string PasswordHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string PBKDF2Hash(string password, byte[] salt)
        {
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = rfc2898DeriveBytes.GetBytes(20);
                return System.Convert.ToBase64String(hash);
            }
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public string BCryptHashing(string textToHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(textToHash);
           
        }

        public bool BCryptVerify(string textToVerify, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(textToVerify, hashedText);
        }

        

    }
}
