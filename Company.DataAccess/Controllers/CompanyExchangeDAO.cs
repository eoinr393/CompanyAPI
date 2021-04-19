using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    /// <summary>
    /// Database operations involving the company_exchange join table
    /// </summary>
    public class CompanyExchangeDAO : ICompanyExchangeDAO
    {
        private readonly CompanyDBContext dbContext;

        public CompanyExchangeDAO(CompanyDBContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Creates a new entry in the company_exchange table
        /// </summary>
        /// <param name="companyExchange">the to-be-created company</param>
        /// <returns>the created object</returns>
        public async Task<CompanyExchange> CreateCompanyExchange(CompanyExchange companyExchange)
        {
            await dbContext.CompanyExchange.AddAsync(companyExchange);
            dbContext.SaveChanges();
            return companyExchange; 
        }

        /// <summary>
        /// Gets a list of companyExchanges by their CompanyId
        /// </summary>
        /// <param name="id">the companyId</param>
        /// <returns>a list of companyExchanges </returns>
        public async Task<List<CompanyExchange>> GetCompanyExchangesByCompanyId(int id)
        {
            return await dbContext.CompanyExchange.Where(c => c.CompanyId == id).ToListAsync();
        }

        /// <summary>
        /// Updates the ExchangeIDs of a given CopmanyID
        /// </summary>
        /// <param name="companyId">the CompanyId</param>
        /// <param name="exchangeIds">the new exchange Id's that should be related to the companyId</param>
        /// <returns>The updated list of company exchanges</returns>
        public async Task<List<CompanyExchange>> UpdateCompanyExchanges(int companyId, List<int> exchangeIds)
        {
            //Get current existing
            var companyExchanges = await this.GetCompanyExchangesByCompanyId(companyId);

            //Exchange Ids in CompanyExchange that be removed as were deleted/overwritten           
            List<CompanyExchange> removedCompanyExchanges = companyExchanges.Where(e => !exchangeIds.Contains(e.ExchangeId)).ToList();            

            dbContext.RemoveRange(removedCompanyExchanges);

            //Newly added exchange Ids
            List<int> newCompanyExchanges = exchangeIds.Where(e => !companyExchanges.Select(ce => ce.ExchangeId).ToList().Contains(e)).ToList();
            List<CompanyExchange> newExchanges = new List<CompanyExchange>();
            foreach(int exchangeId in newCompanyExchanges)
            {
                newExchanges.Add(new CompanyExchange { CompanyId = companyId, ExchangeId = exchangeId });
            }

            await dbContext.AddRangeAsync(newExchanges);

            await dbContext.SaveChangesAsync();
            return await this.GetCompanyExchangesByCompanyId(companyId);
        }
    }
}
