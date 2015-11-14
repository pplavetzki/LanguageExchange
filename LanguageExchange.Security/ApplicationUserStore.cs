using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LanguageExchange.Security
{
    public class ApplicatonUserStore :
        UserStore<ApplicationUser>, IApplicationUserStore
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
