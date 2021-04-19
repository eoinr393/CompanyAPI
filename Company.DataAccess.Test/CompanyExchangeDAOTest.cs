using Company.DataAccess.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DataAccess.Test
{
    /// <summary>
    /// Tests relating to CompanyExchangeDAO
    /// </summary>
    [TestFixture]
    public class CompanyExchangeDAOTest
    {
        private ICompanyExchangeDAO dao;

        private DbContextOptions<CompanyDBContext> options = new DbContextOptionsBuilder<CompanyDBContext>().UseInMemoryDatabase(databaseName: "CompanyExchangeDatabase").Options;

        [SetUp]
        public void SetUp()
        {
            // Insert seed data into the database using one instance of the context
            using (var context = new CompanyDBContext(options))
            {
                context.Database.EnsureDeleted();

                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 1, ExchangeId = 1 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 1, ExchangeId = 2 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 2, ExchangeId = 3 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 2, ExchangeId = 4 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 2, ExchangeId = 5 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 3, ExchangeId = 2 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 3, ExchangeId = 3 });
                context.CompanyExchange.Add(new Models.CompanyExchange { CompanyId = 3, ExchangeId = 4 });              

                context.SaveChanges();
            }

            dao = new CompanyExchangeDAO(new CompanyDBContext(options));
        }

        [TestCase(1, new int[] { 1, 2 })]
        [TestCase(2, new int[] { 3, 4, 5 })]
        [TestCase(3, new int[] { 2, 3, 4 })]
        public async Task GetCompanyExchangesByCompanyId(int companyId, int[] expectedExchanges)
        {
            var ret = await dao.GetCompanyExchangesByCompanyId(companyId);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedExchanges, ret.Select(r => r.ExchangeId).ToArray());
            Assert.Pass();
        }


        [TestCase(4, 4)]
        [TestCase(1, 5)]
        public async Task CreateCompanyExchange(int companyId, int exchangeId)
        {
            await dao.CreateCompanyExchange(new Models.CompanyExchange
            {
                CompanyId = companyId,
                ExchangeId = exchangeId
            });

            var ret = await dao.GetCompanyExchangesByCompanyId(companyId);

            Assert.NotNull(ret);
            Assert.NotNull(ret.First(r => r.ExchangeId == exchangeId));
            Assert.Pass();
        }

        [TestCase(1, new int[] { 1, 2, 3 })]
        [TestCase(2, new int[] { 1 })]
        [TestCase(3, new int[] { 1, 2, 6 })]
        public async Task UpdateCompanyExchange(int companyId, int[] newExchangeIds)
        {
            var ret = await dao.UpdateCompanyExchanges(companyId, newExchangeIds.ToList());

            var sortedArray = ret.Select(e => e.ExchangeId).ToArray().OrderBy(i => i);

            Assert.NotNull(ret);
            Assert.AreEqual(newExchangeIds, sortedArray);
            Assert.Pass();
        }
    }
}
