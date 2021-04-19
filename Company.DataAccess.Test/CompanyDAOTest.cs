using Company.DataAccess.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Company.DataAccess.Test
{
    /// <summary>
    /// Tests relating to Company DAO
    /// </summary>
    [TestFixture]
    public class CompanyDAOTest
    {
        private ICompanyDAO dao;

        private DbContextOptions<CompanyDBContext> options = new DbContextOptionsBuilder<CompanyDBContext>().UseInMemoryDatabase(databaseName: "CompanyDatabase").Options;

        [SetUp]
        public void Setup()
        {
            // Insert seed data into the database using one instance of the context
            using (var context = new CompanyDBContext(options))
            {
                context.Database.EnsureDeleted();

                context.Companys.Add(new Models.Company { Id = 1, Name = "Company1", ISIN="ER00001", Website = "test1.com" });
                context.Companys.Add(new Models.Company { Id = 2, Name = "Company2", ISIN = "ER00002", Website = "test2.com" });
                context.Companys.Add(new Models.Company { Id = 3, Name = "Company3", ISIN = "ER00003", Website = "test3.com" });
                context.Companys.Add(new Models.Company { Id = 4, Name = "Company4", ISIN = "ER00004", Website = "test4.com" });
                context.SaveChanges();
            }
            
            dao = new CompanyDAO(new CompanyDBContext(options));
        }

        [Test]
        public async Task GetCompanys()
        {
            var ret = await this.dao.GetCompanys();

            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Count, 4);
            Assert.Pass();
        }

        [TestCase(1, "Company1")]
        [TestCase(2, "Company2")]
        public async Task GetCompanyById(int id, string expectedCompanyName)
        {
            var ret = await this.dao.GetCompanyById(id);

            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Name, expectedCompanyName);
            Assert.Pass();
        }

        [TestCase("ER00001", "Company1")]
        [TestCase("ER00002", "Company2")]
        public async Task GetCompanyByISIN(string isin, string expectedCompanyName)
        {
            var ret = await this.dao.GetCompanyByISIN(isin);

            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Name, expectedCompanyName);
            Assert.Pass();
        }

        [TestCase( "Company5","test5.com", "ER00005")]
        [TestCase( "Company6","test6.com", "ER00006")]
        public async Task CreateCompany(string name, string website, string isin)
        {
            var ret = await this.dao.CreateCompany(new Models.Company { 
            Name=name,
            Website =website,
            ISIN = isin
            });

            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Name, name);
            Assert.AreEqual(ret.Website, website);
            Assert.AreEqual(ret.ISIN, isin);
            Assert.Pass();
        }


        [TestCase(1, "Company1", "test7.com", "ER00007")]
        [TestCase(2, "Company2", "test8.com", "ER00008")]
        public async Task UpdateCompany(int id, string name, string website, string isin)
        {
            await this.dao.UpdateCompany(new Models.Company
            {
                Id=id,
                Name = name,
                Website = website,
                ISIN = isin
            });

            var ret = await this.dao.GetCompanyById(id);

            Assert.NotNull(ret);
            Assert.AreEqual(ret.ISIN, isin);
            Assert.AreEqual(ret.Name, name);
            Assert.AreEqual(ret.Website, website);
            Assert.Pass();
        }
    }
}