using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExchange.Models;

namespace LanguageExchange.Interfaces
{
    public interface IUserDetailRepository
    {
        IQueryable<UserDetail> GetUserDetailDeep();
    }
}
