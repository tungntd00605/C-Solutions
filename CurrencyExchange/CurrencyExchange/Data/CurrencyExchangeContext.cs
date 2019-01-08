using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Models
{
    public class CurrencyExchangeContext : DbContext
    {
        public CurrencyExchangeContext (DbContextOptions<CurrencyExchangeContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyExchange.Models.Currency> Currency { get; set; }
    }
}
