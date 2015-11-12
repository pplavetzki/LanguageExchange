using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LanguageExchange.Controllers
{
    public class BaseApiController : ApiController
    {
        protected DocumentClient _dbClient = null;

        public BaseApiController(DocumentClient dbClient)
        {
            _dbClient = dbClient;
        }

        protected override void Dispose(bool disposing)
        {
            _dbClient.Dispose();
            base.Dispose(disposing);
        }
    }
}
