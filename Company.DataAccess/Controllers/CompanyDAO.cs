using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{
    public class CompanyDAO : ICompanyDAO
    {
        private readonly CompanyDBContext dbContext;

        public CompanyDAO (CompanyDBContext context)
        {
            this.dbContext = context;
        }

        public async Task<Models.Company> CreateCompany(Models.Company company)
        {
            await dbContext.Companys.AddAsync(company);
            dbContext.SaveChanges();
            return await this.GetCompanyByISIN(company.ISIN);
        }

        public async Task<List<Models.Company>> GetCompanys()
        {
            return await dbContext.Companys.ToListAsync();
        }

        public async Task<Models.Company> GetCompanyById(int id)
        {
            return await dbContext.Companys.FindAsync(id);
        }

        public async Task<Models.Company> GetCompanyByISIN(string isin)
        {
            return await dbContext.Companys.FirstOrDefaultAsync(c => c.ISIN == isin);
        }

        public async Task<Models.Company> UpdateCompany(Models.Company company)
        {
            var entity = await this.GetCompanyById(company.Id);

            if (entity != null)
            {
                entity.Name = company.Name;
                entity.ISIN = company.ISIN;
                entity.Website = company.Website;

                dbContext.SaveChanges();
            }

            return entity;
        }
    }
}
