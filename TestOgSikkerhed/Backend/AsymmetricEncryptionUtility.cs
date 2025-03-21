using System;
using System.Security.Cryptography;
using System.Text;

namespace TestOgSikkerhed.Backend
{
    public class AsymmetricEncryptionUtility
    {
        private readonly RSA _rsa;

        public string PublicKey { get; private set; } = string.Empty;
        private readonly string _privateKey;

        public AsymmetricEncryptionUtility()
        {
            _rsa = RSA.Create(2048); // 2048-bit RSA nøgle

            // Eksporter nøgler
            PublicKey = Convert.ToBase64String(_rsa.ExportSubjectPublicKeyInfo());
            _privateKey = Convert.ToBase64String(_rsa.ExportPkcs8PrivateKey());
        }

        public string Encrypt(string plainText)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            using (RSA rsaEncryptor = RSA.Create())
            {
                rsaEncryptor.ImportSubjectPublicKeyInfo(Convert.FromBase64String(PublicKey), out _);
                byte[] encryptedBytes = rsaEncryptor.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public string Decrypt(string encryptedText)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
            using (RSA rsaDecryptor = RSA.Create())
            {
                rsaDecryptor.ImportPkcs8PrivateKey(Convert.FromBase64String(_privateKey), out _);
                byte[] decryptedBytes = rsaDecryptor.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
