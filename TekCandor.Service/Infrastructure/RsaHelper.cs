using System.Security.Cryptography;
using System.Text;

namespace TekCandor.Service.Infrastructure
{
    public static class RsaHelper
    {
        public static string EncryptText(string plainText, string publicKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);

            using RSA rsaEncrypt = RSA.Create();
            rsaEncrypt.FromXmlString(publicKey);

            byte[] encryptedBytes = rsaEncrypt.Encrypt(
                data,
                RSAEncryptionPadding.Pkcs1
            );

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptText(string encrypted, string privateKey)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);

            using RSA rsaDecrypt = RSA.Create();
            rsaDecrypt.FromXmlString(privateKey);

            byte[] decryptedBytes = rsaDecrypt.Decrypt(
                encryptedBytes,
                RSAEncryptionPadding.Pkcs1
            );

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
