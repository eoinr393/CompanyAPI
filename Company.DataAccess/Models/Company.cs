using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.DataAccess.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISIN { get; set; }
        public string Website { get; set; }


        public ICollection<CompanyExchange> CompanyExchange { get; set; }
        public ICollection<Ticker> Ticker { get; set; }
    }
}
