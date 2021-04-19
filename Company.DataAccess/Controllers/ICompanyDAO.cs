using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public interface ICompanyDAO
    {
        Task<Models.Company> CreateCompany(Models.Company company);
        Task<List<Models.Company>> GetCompanys();
        Task<Models.Company> GetCompanyByISIN(string isin);
        Task<Models.Company> GetCompanyById(int id);
        Task<Models.Company> UpdateCompany(Models.Company company);
    }
}
