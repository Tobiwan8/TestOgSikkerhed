namespace TestOgSikkerhed.EncryptionAPI
{
    public class PublicKeyStore
    {
        private static string publicKeyPath = "PublicKey.txt";

        public static string GetPublicKey()
        {
            if (File.Exists(publicKeyPath))
            {
                return File.ReadAllText(publicKeyPath);
            }
            return null;
        }
    }
}
