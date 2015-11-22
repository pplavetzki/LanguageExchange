using LanguageExchange.Interfaces;
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
        private IRedisRepository _redis;

        public CommonController(IRedisRepository redis)
        {
            _redis = redis;
        }

        [Route("countries")]
        // GET: api/common/countries
        public async Task<IHttpActionResult> GetCountries()
        {
            var countries = await _redis.GetCountries();

            return Ok(countries);
        }

        [Route("languages")]
        // GET: api/common/languages
        public async Task<IHttpActionResult> GetLanguages()
        {
            var languages = await _redis.GetLanguages();

            return Ok(languages);
        }
    }
}
