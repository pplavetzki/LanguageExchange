using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageExchange.Models
{
    public interface IApplicationUserStore : IUserStore<ApplicationUser, int>
    {

    }
}
