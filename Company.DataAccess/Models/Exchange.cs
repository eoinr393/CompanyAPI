using System.Collections.Generic;

namespace Company.DataAccess.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CompanyExchange> CompanyExchange { get; set; }
    }
}
