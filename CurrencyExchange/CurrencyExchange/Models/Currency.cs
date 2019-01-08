using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchange.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public double ExchangeRate { get; set; }
    }
}
