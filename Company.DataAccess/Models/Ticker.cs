
namespace Company.DataAccess.Models
{
    public class Ticker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public Company Company {get;set;}
    }
}
