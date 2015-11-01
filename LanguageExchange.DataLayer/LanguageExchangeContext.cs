using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LanguageExchange.Models;

namespace LanguageExchange.DataLayer
{
    public class LanguageExchangeContext : DbContext
    {
        public LanguageExchangeContext() : base("LanguageExchange.Data")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<UserDetail> UserDetails { get; set; }
    }
}
