using LanguageExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Interfaces
{
    public interface IRegistrationProcess
    {
        void RegisterUser(ApplicationUser user);
    }
}
