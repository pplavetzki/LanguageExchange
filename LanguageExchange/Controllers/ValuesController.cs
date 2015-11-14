using LanguageExchange.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;
using LanguageExchange.Repository;
using LanguageExchange.Security;
using LanguageExchange.Models.Dtos;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace LanguageExchange.Controllers
{
    //[ResourceAuthorize("access", "full", "read", "any")]
    [Security.ScopeAuthorize("full", "read", "any")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/values
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]UserDto value)
        {
            if (value.Email != null)
            {
                using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("10.211.55.65:6379"))
                {
                    var redisRepo = new RedisRepository(redis);
                    await redisRepo.InsertNewUser(value);
                }

                return Ok();
            }

            return InternalServerError();
        }
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
