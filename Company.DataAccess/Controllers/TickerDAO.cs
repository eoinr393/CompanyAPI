using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public class TickerDAO : ITickerDAO
    {
        private readonly CompanyDBContext dbContext;

        public TickerDAO(CompanyDBContext context)
        {
            this.dbContext = context;
        }

        public async Task<Ticker> CreateTicker(Ticker ticker)
        {
            var existing = await this.GetTickerByName(ticker.Name);
            if (existing != null)
                return existing;

            await dbContext.Tickers.AddAsync(ticker);
            dbContext.SaveChanges();
            return await this.GetTickerByName(ticker.Name);
        }

        public async Task<Ticker> GetTickerByTickerId(int id)
        {
            return await dbContext.Tickers.FindAsync(id);
        }

        public async Task<Ticker> GetTickerByName(string name)
        {
            return await dbContext.Tickers.FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<List<Ticker>> GetTickersByCompanyId(int id)
        {
            return await dbContext.Tickers.Where(t => t.CompanyId == id).ToListAsync();
        }

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
