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
        private IConnectionMultiplexer _redis;

        public RedisRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task InsertRefreshTokenAsync(RefreshTokenDto tokenDto)
        {
            IDatabase db = _redis.GetDatabase();
            string refreshKey = "refresh:" + tokenDto.Subject;

            var refreshValue = await db.StringGetAsync(refreshKey);
            if (!string.IsNullOrEmpty(refreshValue))
            {
                await db.KeyDeleteAsync(refreshValue.ToString());
            }
            string userValue = await JsonConvert.SerializeObjectAsync(tokenDto);

            await db.StringSetAsync(tokenDto.Id, userValue);
            await db.StringSetAsync(refreshKey, tokenDto.Id);
        }

        public async Task<RefreshTokenDto> GetRefreshTokenAsync(string tokenId)
        {
            IDatabase db = _redis.GetDatabase();

            var token = await db.StringGetAsync(tokenId);
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var refreshToken = await JsonConvert.DeserializeObjectAsync<RefreshTokenDto>(token);

            return refreshToken;
        }

        public async Task InsertNewUser(UserDto user)
        {
            IDatabase db = _redis.GetDatabase();

            string userKey = "user:" + user.Id.ToString();
            string userValue = await JsonConvert.SerializeObjectAsync(user);

            await db.StringSetAsync(userKey, userValue);
        }

        public async Task InsertMostRecentUser(MostRecentUserDto user)
        {
            IDatabase db = _redis.GetDatabase();

            string userValue = await JsonConvert.SerializeObjectAsync(user);

            await db.ListLeftPushAsync("MostRecent", userValue);
            await db.ListTrimAsync("MostRecent", 0, 4);
        }

        public async Task<string[]> GetLanguages()
        {
            IDatabase db = _redis.GetDatabase();

            var languages = await db.ListRangeAsync("languages", 0, -1);
            var results = Array.ConvertAll<RedisValue, string>(languages, language => (string)language);

            return results;
        }

        public async Task<string[]> GetCountries()
        {
            IDatabase db = _redis.GetDatabase();

            var countries = await db.ListRangeAsync("countries", 0, -1);
            var results = Array.ConvertAll<RedisValue, string>(countries, country => (string)country);

            return results;
        }

        public async Task<bool> RemoveRefreshToken(string tokenId, string subject)
        {
            IDatabase db = _redis.GetDatabase();

            await db.KeyDeleteAsync("refresh:" + subject);
            return await db.KeyDeleteAsync(tokenId);
        }
    }
}
