using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace BlazorAppTest.Hashing
{
    public class SymtriskHandler
    {
        private readonly IDataProtector _dataProtector;

        public SymtriskHandler(IDataProtectionProvider protector) 
        { 
            _dataProtector = protector.CreateProtector("BlazorAppTest.Hashing.SymtriskHandler");
        }

        public string protect(string textToProtect)
        {
            return _dataProtector.Protect(textToProtect);
        }

        public string unprotect(string textToUnprotect)
        {
            return _dataProtector.Unprotect(textToUnprotect);
        }



        public string Encrypt(string textToEncrypt)
        {

            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(textToEncrypt);
                        }
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string textToDecrypt)
        {
            byte[] buffer = Convert.FromBase64String(textToDecrypt);
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
