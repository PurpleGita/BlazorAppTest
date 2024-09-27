
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace BlazorAppTest.Hashing
{
    public class AsymtriskHandlercs
    {
        private string _privateKey;
        private string _publicKey;
        private readonly HttpClient _httpClient;

        public AsymtriskHandlercs(HttpClient httpClient)
        {
            _httpClient = httpClient;

            using (RSA rsa = RSA.Create())
            {
                _privateKey = rsa.ToXmlString(true);
                _publicKey = rsa.ToXmlString(false);
            }

            _httpClient = httpClient;
        }

        public string AsyncEncrypt(string textToEncrypt)
        {
            byte[] encrypted;
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(_publicKey);
                encrypted = rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(textToEncrypt), RSAEncryptionPadding.OaepSHA256);
            }
            return Convert.ToBase64String(encrypted);
        }

        public async Task<string> AsyncEncryptAgain(string textToEncrypt)
        {
            string[] param = new string[] { textToEncrypt, _publicKey };
            string serializedObject = JsonConvert.SerializeObject(param);
            StringContent sc = new StringContent(serializedObject);

            var response = await _httpClient.PostAsync("https://localhost:7089/Api/Encrypter", sc);
            string encryptedValueAsString = response.Content.ReadAsStringAsync().Result;
            return encryptedValueAsString;
        }

        public string AsyncDecrypt(string textToDecrypt)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(_publicKey);
                byte[] buffer = Convert.FromBase64String(textToDecrypt);
                byte[] decryptValue = rsa.Decrypt(buffer, RSAEncryptionPadding.OaepSHA256);
                string decryptValueAsString = System.Text.Encoding.UTF8.GetString(decryptValue);

                return decryptValueAsString;
            }
        }

    }


}
