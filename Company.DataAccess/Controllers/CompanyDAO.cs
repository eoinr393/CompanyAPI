using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.DataAccess.Controllers
{ 
    /// <summary>
    /// Database operations involving the company DB
    /// </summary>
    public class CompanyDAO : ICompanyDAO
    {
        private readonly CompanyDBContext dbContext;

        public CompanyDAO (CompanyDBContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Create a Company
        /// </summary>
        /// <param name="company"></param>
        /// <returns>The Created Company</returns>
        public async Task<Models.Company> CreateCompany(Models.Company company)
        {
            await dbContext.Companys.AddAsync(company);
            dbContext.SaveChanges();
            return await this.GetCompanyByISIN(company.ISIN);
        }

        /// <summary>
        /// Gets all companys
        /// </summary>
        /// <returns>a list of all companys</returns>
        public async Task<List<Models.Company>> GetCompanys()
        {
            return await dbContext.Companys.ToListAsync();
        }

        /// <summary>
        /// Gets a company by its companyId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the company</returns>
        public async Task<Models.Company> GetCompanyById(int id)
        {
            return await dbContext.Companys.FindAsync(id);
        }

        /// <summary>
        /// Gets a company by its ISIN
        /// </summary>
        /// <param name="isin"></param>
        /// <returns>the company</returns>
        public async Task<Models.Company> GetCompanyByISIN(string isin)
        {
            return await dbContext.Companys.FirstOrDefaultAsync(c => c.ISIN == isin);
        }

        /// <summary>
        /// Updates a copmany's Name, ISIN and Website values
        /// </summary>
        /// <param name="company"></param>
        /// <returns>the updated company</returns>
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
