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
    }
}
