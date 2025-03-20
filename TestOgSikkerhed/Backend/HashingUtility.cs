using System;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace TestOgSikkerhed.Backend
{
    public class HashingUtility
    {
        // SHA2 (SHA-256) Hashing Method
        public T ComputeSHA256<T>(T textToHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                if (textToHash is string textString)
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(textString));
                    return (T)(object)Convert.ToHexString(hashBytes);
                }
                else if (textToHash is byte[] textBytes)
                {
                    return (T)(object)sha256.ComputeHash(textBytes);
                }
                else
                {
                    throw new ArgumentException("Invalid input type. Use string or byte[].");
                }
            }
        }

        // HMAC (HMAC-SHA256) Hashing Method
        public T ComputeHMAC<T>(T textToHash, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                if (textToHash is string textString)
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(textString));
                    return (T)(object)Convert.ToHexString(hashBytes);
                }
                else if (textToHash is byte[] textBytes)
                {
                    return (T)(object)hmac.ComputeHash(textBytes);
                }
                else
                {
                    throw new ArgumentException("Invalid input type. Use string or byte[].");
                }
            }
        }

        // PBKDF2 Hashing Method
        public string ComputePBKDF2(string textToHash, byte[] salt, int iterations = 10000)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(textToHash, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public byte[] GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // BCrypt Hashing Method (Always returns a string since BCrypt produces a hash with salt)
        public string ComputeBCrypt(string textToHash, int workFactor = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(textToHash, workFactor);
        }
    }
}
