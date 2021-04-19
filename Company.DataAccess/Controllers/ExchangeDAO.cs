using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    /// <summary>
    /// Database operations relating to the exchange table
    /// </summary>
    public class ExchangeDAO : IExchangeDAO
    {
        private readonly CompanyDBContext dbContext;

        public ExchangeDAO(CompanyDBContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Creates a new Exchange
        /// </summary>
        /// <param name="exchange">The new exchange to be created</param>
        /// <returns>the newly create</returns>
        public async Task<Exchange> CreateExchange(Exchange exchange)
        {
            //if exchange already exists just return that exchange as exchange names are unique
            var existingExchange = await this.GetExchangeByName(exchange.Name);
            if (existingExchange != null)           
                return existingExchange;            

            await dbContext.Exchange.AddAsync(exchange);
            dbContext.SaveChanges();
            return await this.GetExchangeByName(exchange.Name);
        }

        /// <summary>
        /// Gets an exchange by its ExchangeID
        /// </summary>
        /// <param name="id">the exchange id</param>
        /// <returns>the exchange</returns>
        public async Task<Exchange> GetExchangeById(int id)
        {
            return await dbContext.Exchange.FindAsync(id);
        }

        /// <summary>
        /// Gets an Exchange by its Name
        /// </summary>
        /// <param name="name">the exchange name</param>
        /// <returns>the exchange</returns>
        public async Task<Exchange> GetExchangeByName(string name)
        {
            return await dbContext.Exchange.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
