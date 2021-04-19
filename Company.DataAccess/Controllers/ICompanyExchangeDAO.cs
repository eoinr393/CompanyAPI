using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public interface ICompanyExchangeDAO
    {
        Task<Models.CompanyExchange> CreateCompanyExchange(Models.CompanyExchange companyExchange);
        Task<List<Models.CompanyExchange>> GetCompanyExchangesByCompanyId(int id);
        Task<List<Models.CompanyExchange>> UpdateCompanyExchanges(int companyId, List<int> exchanges);
    }
}
