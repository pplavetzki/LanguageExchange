using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using LanguageExchange.Interfaces;

namespace LanguageExchange.Models
{
    public class ApplicatonUserStore :
        UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IApplicationUserStore
    {
        public ApplicatonUserStore(ApplicationDbContext context)
            : base(context)
        {
        }

        public ApplicatonUserStore() : base(new ApplicationDbContext())
        {

        }
    }
}
