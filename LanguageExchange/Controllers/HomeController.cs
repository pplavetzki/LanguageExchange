using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageExchange.Common;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Digests;
using System.Configuration;

namespace LanguageExchange.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var cipher = ConfigurationManager.AppSettings["cipherKey"];
            var hmacKey = ConfigurationManager.AppSettings["hmacKey"];

            byte[] key = Convert.FromBase64String(cipher);
            byte[] hmac = Convert.FromBase64String(hmacKey);

            //var encryptor = new Encryptor<AesEngine, Sha256Digest>(Encoding.UTF8, key, hmac);

            ViewBag.OriginalString = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //ViewBag.EncryptedString = encryptor.Encrypt(ViewBag.OriginalString);
            //ViewBag.DecryptedString = encryptor.Decrypt(ViewBag.EncryptedString);

            //ViewBag.EncryptedString = AESGCM.SimpleEncrypt(ViewBag.OriginalString, key, hmac);
            //ViewBag.DecryptedString = AESGCM.SimpleDecrypt("DtQn4vu+Zzl6GjJSpz8IM1+UxDvGkPsSZBsTEcjQ4EdPV+/KkF4p1Z2e3mdhR9UxB0DLp3ddUbggzoOp+Hp43pr0tioBqyE0NCDPm+UNoDmWFCEmXjYnBDRPgNafjqaKZIg5qe/rra/f", key, hmac.Length);


            return View();
        }
    }
}
