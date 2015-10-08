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

            var encryptor = new Encryptor<AesEngine, Sha256Digest>(Encoding.UTF8, key, hmac);

            ViewBag.OriginalString = "070704115";
            ViewBag.EncryptedString = encryptor.Encrypt(ViewBag.OriginalString);
            ViewBag.DecryptedString = encryptor.Decrypt(ViewBag.EncryptedString);

            return View();
        }
    }
}
