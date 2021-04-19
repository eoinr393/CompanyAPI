using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    /// <summary>
    /// Database operations relating to the ticker table
    /// </summary>
    public class TickerDAO : ITickerDAO
    {
        private readonly CompanyDBContext dbContext;

        public TickerDAO(CompanyDBContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Creates a Ticker
        /// </summary>
        /// <param name="ticker">the ticker to be created</param>
        /// <returns>the ticker</returns>
        public async Task<Ticker> CreateTicker(Ticker ticker)
        {
            //If ticker already exists return that ticker as ticker names are unique
            var existing = await this.GetTickerByName(ticker.Name);
            if (existing != null)
                return existing;

            await dbContext.Tickers.AddAsync(ticker);
            dbContext.SaveChanges();
            return await this.GetTickerByName(ticker.Name);
        }

        /// <summary>
        /// Gets a ticker by its Id
        /// </summary>
        /// <param name="id">the id of the ticker</param>
        /// <returns>the ticker</returns>
        public async Task<Ticker> GetTickerByTickerId(int id)
        {
            return await dbContext.Tickers.FindAsync(id);
        }

        /// <summary>
        /// Gets a Ticker by its name
        /// </summary>
        /// <param name="name">the name of the ticker</param>
        /// <returns>the ticker</returns>
        public async Task<Ticker> GetTickerByName(string name)
        {
            return await dbContext.Tickers.FirstOrDefaultAsync(t => t.Name == name);
        }

        /// <summary>
        /// Gets a list of tickers relating to a given company by its companyId
        /// </summary>
        /// <param name="id">the companyId</param>
        /// <returns>a list of tickers</returns>
        public async Task<List<Ticker>> GetTickersByCompanyId(int id)
        {
            return await dbContext.Tickers.Where(t => t.CompanyId == id).ToListAsync();
        }

        /// <summary>
        /// Updates the tickers relating to a given companyId adding new ones and removing ones no longer associated with company
        /// </summary>
        /// <param name="companyId">the companyId to update</param>
        /// <param name="tickersNames">the names of the new tickers</param>
        /// <returns>List of updated tickers</returns>
        public async Task<List<Ticker>> UpdateCompanyTickers(int companyId, List<string> tickersNames)
        {
            List<Ticker> currentTickers = await this.GetTickersByCompanyId(companyId);

            List<Ticker> removedTickers = currentTickers.Where(c => !tickersNames.Contains(c.Name)).ToList();

            dbContext.RemoveRange(removedTickers);

            List<string> newTickerNames = tickersNames.Except(currentTickers.Select(ct => ct.Name)).ToList();
            List<Ticker> newTickers = new List<Ticker>();

            foreach (string s in newTickerNames)
            {
                newTickers.Add(new Ticker
                {
                    Name = s,
                    CompanyId = companyId
                });

            }

            await dbContext.AddRangeAsync(newTickers);

            dbContext.SaveChanges();

            return await this.GetTickersByCompanyId(companyId);
        }
    }
}
