using System.Collections.Generic;
using System.Linq;

namespace CompanyAPI.Models
{
    /// <summary>
    /// Mapper for CompanyDAO and Company Model objects
    /// </summary>
    public class CompanyMapper
    {
        /// <summary>
        /// Creates a CompanyModel object from given DAO models
        /// </summary>
        /// <param name="company">the company model</param>
        /// <param name="exchanges">list of exchanges</param>
        /// <param name="tickers">list of tickers</param>
        /// <returns></returns>
        public static CompanyModel DAOToAPIModel(Company.DataAccess.Models.Company company,
            List<Company.DataAccess.Models.Exchange> exchanges =null,
            List<Company.DataAccess.Models.Ticker> tickers = null)
        {

            tickers ??= new List<Company.DataAccess.Models.Ticker>();
            exchanges ??= new List<Company.DataAccess.Models.Exchange>();

            var companyModel = new CompanyModel
            {
                Id = company.Id,
                Name = company.Name,
                ISIN = company.ISIN,
                Website = company.Website,
                Ticker = tickers.Select(t => t.Name).ToList(),
                CompanyExchange = exchanges.Select(e => e.Name).ToList()
            };

            return companyModel;
        }
    }
}
