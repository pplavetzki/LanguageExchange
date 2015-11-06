using LanguageExchange.Interfaces;
using LanguageExchange.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Repository
{
    public class UserRepository : BaseRepository<ApplicationUser>
    {
        private IRegistrationProcess _registrationProcess;

        public UserRepository(DbContext context, IRegistrationProcess regProcess) : base(context)
        {
            _registrationProcess = regProcess;
        }


    }
}
