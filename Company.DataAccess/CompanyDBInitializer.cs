using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DataAccess
{
    /// <summary>
    /// Initialses the Database with seed data
    /// </summary>
    public static class CompanyDBInitializer
    {        
        public static void Initialize(CompanyDBContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Companys.Any())
            {
                List<Models.Exchange> exchanges = new List<Models.Exchange>()
                {
                    new Models.Exchange{ Name= "NASDAQ"},
                    new Models.Exchange{ Name= "Pink Sheets"},
                    new Models.Exchange{ Name= "Euronext Amsterdam"},
                    new Models.Exchange{ Name= "Tokyo Stock Exchange"},
                    new Models.Exchange{ Name= "Deutsche Börse"},
                };

                context.Exchange.AddRange(exchanges);
                context.SaveChanges();

                List<Models.Company> companies = new List<Models.Company>
                {
                    new Models.Company{ Name = "Apple Inc.", ISIN = "US0378331005", Website = "http://www.apple.com"},
                    new Models.Company{ Name = "British Airways Plc", ISIN = "US1104193065", Website = null},
                    new Models.Company{ Name = "Heineken NV",  ISIN = "NL0000009165", Website = null },
                    new Models.Company{ Name = "Panasonic Corp",  ISIN = "JP3866800000", Website = "http://www.panasonic.co.jp"},
                    new Models.Company{ Name = "Porsche Automobil ", ISIN = "DE000PAH0038", Website = "https://www.porsche.com/"},
                };

                context.Companys.AddRange(companies);
                context.SaveChanges();

                List<Models.CompanyExchange> companyExchanges = new List<Models.CompanyExchange>
                {
                    new Models.CompanyExchange{CompanyId = 1, ExchangeId = 1},
                    new Models.CompanyExchange{CompanyId = 2, ExchangeId = 2},
                    new Models.CompanyExchange{CompanyId = 3, ExchangeId = 3},
                    new Models.CompanyExchange{CompanyId = 4, ExchangeId = 4},
                    new Models.CompanyExchange{CompanyId = 5, ExchangeId = 5}
                };
                

                context.CompanyExchange.AddRange(companyExchanges);
                context.SaveChanges();

                List<Models.Ticker> tickers = new List<Models.Ticker>()
                {
                    new Models.Ticker{ Name= "AAPL", CompanyId = 1},
                    new Models.Ticker{ Name= "BAIRY", CompanyId = 2},
                    new Models.Ticker{ Name= "HEIA", CompanyId = 3},
                    new Models.Ticker{ Name= "6752", CompanyId = 4},
                    new Models.Ticker{ Name= "PAH3", CompanyId = 5},
                };

                context.Tickers.AddRange(tickers);
                context.SaveChanges();
            }
        }
    }
}
