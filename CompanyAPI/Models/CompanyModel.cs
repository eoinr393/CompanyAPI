using System.Collections.Generic;

namespace CompanyAPI.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISIN { get; set; }
        public string Website { get; set; }

        public List<string> CompanyExchange { get; set; }
        public List<string> Ticker { get; set; }
    }
}
