using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public class ExchangeDAO : IExchangeDAO
    {
        private readonly CompanyDBContext dbContext;

        public ExchangeDAO(CompanyDBContext context)
        {
            this.dbContext = context;
        }

        public async Task<Exchange> CreateExchange(Exchange exchange)
        {
            var existingExchange = await this.GetExchangeByName(exchange.Name);
            if (existingExchange != null)           
                return existingExchange;            

            await dbContext.Exchange.AddAsync(exchange);
            dbContext.SaveChanges();
            return await this.GetExchangeByName(exchange.Name);
        }

        public async Task<Exchange> GetExchangeById(int id)
        {
            return await dbContext.Exchange.FindAsync(id);
        }

        public async Task<Exchange> GetExchangeByName(string name)
        {
            return await dbContext.Exchange.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
