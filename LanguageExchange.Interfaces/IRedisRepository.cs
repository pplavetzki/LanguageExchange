using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExchange.Models.Dtos;

namespace LanguageExchange.Interfaces
{
    public interface IRedisRepository
    {
        Task InsertNewUser(UserDto user);
        Task InsertMostRecentUser(MostRecentUserDto user);
        Task<string[]> GetLanguages();
        Task<string[]> GetCountries();
        Task InsertRefreshTokenAsync(RefreshTokenDto refreshToken);
        Task<RefreshTokenDto> GetRefreshTokenAsync(string tokenId);
        Task<bool> RemoveRefreshToken(string tokenId, string subject);
    }
}
