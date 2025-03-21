using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestOgSikkerhed.Backend
{
    public class SymmetricEncryptionUtility
    {
        private readonly byte[] _privateKey;

        public SymmetricEncryptionUtility()
        {
            // Brug en fast nøgle (i en rigtig app bør dette gemmes sikkert, f.eks. i en secrets manager)
            _privateKey = Encoding.UTF8.GetBytes("ThisIsASecretKey123"); // Skal være 16, 24 eller 32 bytes lang
        }

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _privateKey;
                aesAlg.GenerateIV(); // Unik IV pr. kryptering
                byte[] iv = aesAlg.IV;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Returnér IV + krypteret data (IV skal bruges til dekryptering)
                    byte[] result = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            byte[] fullCipher = Convert.FromBase64String(encryptedText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _privateKey;

                // Hent IV fra starten af den krypterede tekst
                byte[] iv = new byte[16];
                byte[] cipherBytes = new byte[fullCipher.Length - 16];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
