using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DataAccess.Models
{
    public class CompanyExchange
    {
        public int CompanyId { get; set; }
        public int ExchangeId { get; set; }

        public Company Company { get; set; }
        public Exchange Exchange { get; set; }
    }
}
