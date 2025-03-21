using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace TestOgSikkerhed.EncryptionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private static RSA rsa = RSA.Create();
        //private static string? privateKey;
        //private static string? publicKey;

        //public EncryptionController()
        //{
        //    if (privateKey == null || publicKey == null)
        //    {
        //        rsa.KeySize = 2048; 
        //        privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
        //        publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        //    }
        //}

        [HttpGet("getPublicKey")]
        public ActionResult<string> GetPublicKey()
        {
            return Ok(PublicKeyStore.GetPublicKey());
        }

        //[HttpGet("getPrivateKey")]
        //public ActionResult<string> GetPrivateKey()
        //{
        //    return Ok(Convert.ToBase64String(rsa.ExportPkcs8PrivateKey()));
        //}

        [HttpPost("encrypt")]
        public ActionResult<string> Encrypt([FromBody] EncryptionRequest request)
        {
            try
            {
                // Hent den gemte public key
                string publicKey = PublicKeyStore.GetPublicKey();
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);

                // Krypter teksten
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(request.PlainText);
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);

                return Ok(Convert.ToBase64String(encryptedData));
            }
            catch (Exception ex)
            {
                return BadRequest($"Fejl ved kryptering: {ex.Message}");
            }
        }
    }

    public class EncryptionRequest
    {
        public string PlainText { get; set; }
    }
}
