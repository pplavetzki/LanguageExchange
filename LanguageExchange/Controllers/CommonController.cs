using LanguageExchange.Repository;
using Microsoft.Azure.Documents.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LanguageExchange.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/common")]
    public class CommonController : ApiController
    {
        private IConnectionMultiplexer _redis;
        private DocumentClient _clientDb;

        public CommonController(DocumentClient dbClient, IConnectionMultiplexer redis)
        {
            _redis = redis;
            _clientDb = dbClient;
        }

        [Route("countries")]
        // GET: api/common/countries
        public async Task<IHttpActionResult> GetCountries()
        {
            var redisRepo = new RedisRepository(_redis);
            var countries = await redisRepo.GetCountries();

            return Ok(countries);
        }

        [Route("languages")]
        // GET: api/common/languages
        public async Task<IHttpActionResult> GetLanguages()
        {
            var redisRepo = new RedisRepository(_redis);
            var languages = await redisRepo.GetLanguages();

            return Ok(languages);
        }

        // GET: api/Common/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Common
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Common/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Common/5
        public void Delete(int id)
        {
        }
    }
}
