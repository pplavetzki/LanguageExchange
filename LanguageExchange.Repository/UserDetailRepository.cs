using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExchange.Models;
using System.Data.Entity;
using LanguageExchange.Interfaces;

namespace LanguageExchange.Repository
{
    public class UserDetailRepository : BaseRepository<UserDetail>, IUserDetailRepository
    {
        public UserDetailRepository(DbContext context) : base(context) { }

        public IQueryable<UserDetail> GetUserDetailDeep()
        {
            //return _objectSet.AsQueryable<UserDetail>().Include(ud => ud.LanguageDetails);
            throw new NotImplementedException();            
                                                        
        }
    }
}
