using System.Security.Cryptography;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// Manages Encrypt/Decrypt operations for a string
    /// </summary>
    internal class EncryptionManager
    {
        private const string Key = "E546C8DF278CD5931069B522E695D4F2";

        public static string Encrypt(string text)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(text);
                        }

                        array = ms.ToArray();
                    }
                }

                return Convert.ToBase64String(array);
            }
        }

        public static string Decrypt(string text)
        {
            string result;
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(text);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var ms = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return result;
        }
    }
}

