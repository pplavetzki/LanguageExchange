using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using LanguageExchange.Interfaces;
using LanguageExchange.Models.Dtos;
using Newtonsoft.Json;

namespace LanguageExchange.Repository
{
    //this should be a singleton
    public class RedisRepository : IRedisRepository
    {
        private ConnectionMultiplexer _redis;

        public RedisRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task InsertNewUser(UserDto user)
        {
            IDatabase db = _redis.GetDatabase();

            string userKey = "user:" + user.UserId.ToString();
            string userValue = await JsonConvert.SerializeObjectAsync(user);

            await db.StringSetAsync(userKey, userValue);
        }
    }
}
