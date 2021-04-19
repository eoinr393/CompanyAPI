using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public interface ITickerDAO
    {
        Task<Models.Ticker> CreateTicker(Models.Ticker ticker);
        Task<Models.Ticker> GetTickerByTickerId(int id);
        Task<Models.Ticker> GetTickerByName(string name);
        Task<List<Models.Ticker>> GetTickersByCompanyId(int id);
        Task<List<Models.Ticker>> UpdateCompanyTickers(int companyId, List<String> tickers);
    }
}
