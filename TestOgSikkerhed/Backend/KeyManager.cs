using System.Security.Cryptography;

namespace TestOgSikkerhed.Backend
{
    public static class KeyManager
    {
        private static readonly string keyDirectory = Path.Combine(Directory.GetCurrentDirectory());
        private static readonly string privateKeyPath = Path.Combine(keyDirectory, "Backend/Keys/PrivateKey.txt");
        private static readonly string publicKeyPath = Path.Combine(keyDirectory, "Backend/Keys/PublicKey.txt");
        private static RSA rsa = RSA.Create();

        static KeyManager()
        {
            if (File.Exists(privateKeyPath) && File.Exists(publicKeyPath))
            {
                // Hent gemte nøgler
                rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(File.ReadAllText(privateKeyPath)), out _);
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(File.ReadAllText(publicKeyPath)), out _);
            }
            else
            {
                // Generér og gem nøgler
                rsa.KeySize = 2048;
                File.WriteAllText(privateKeyPath, Convert.ToBase64String(rsa.ExportPkcs8PrivateKey())); // Gem privat nøgle
                File.WriteAllText(publicKeyPath, Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo())); // Gem offentlig nøgle
            }
        }

        public static string GetPrivateKey() => File.ReadAllText(privateKeyPath);
        public static string GetPublicKey() => File.ReadAllText(publicKeyPath);
    }
}
