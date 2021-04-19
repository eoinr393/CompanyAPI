using Company.DataAccess.Controllers;
using Company.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.DataAccess.Test
{
    [TestFixture]
    public class TickerDAOTest
    {
        private ITickerDAO dao;

        private DbContextOptions<CompanyDBContext> options = new DbContextOptionsBuilder<CompanyDBContext>().UseInMemoryDatabase(databaseName: "TickerDatabase").Options;

        [SetUp]
        public void SetUp()
        {
            using(var context = new CompanyDBContext(options))
            {
                context.Database.EnsureDeleted();

                context.Tickers.Add(new Ticker { CompanyId = 1, Name = "AAA" });
                context.Tickers.Add(new Ticker { CompanyId = 1, Name = "GME" });
                context.Tickers.Add(new Ticker { CompanyId = 2, Name = "GOOGL" });
                context.Tickers.Add(new Ticker { CompanyId = 2, Name = "GOOG" });
                context.Tickers.Add(new Ticker { CompanyId = 2, Name = "NFLX" });
                context.Tickers.Add(new Ticker { CompanyId = 3, Name = "G" });
                context.Tickers.Add(new Ticker { CompanyId = 3, Name = "D" });
                context.Tickers.Add(new Ticker { CompanyId = 3, Name = "AAPL" });

                context.SaveChanges();
            }            

            dao = new TickerDAO(new CompanyDBContext(options));
        }

        [TestCase(1, "CSV")]
        [TestCase(2, "AMC")]
        public async Task CreateTicker(int companyId, string name)
        {
            var ret = await dao.CreateTicker(new Ticker
            {
                CompanyId = companyId,
                Name = name
            });

            Assert.NotNull(ret);
            Assert.AreEqual(ret.Name, name);
            Assert.AreEqual(ret.CompanyId, companyId);
            Assert.Pass();
        }


        [TestCase(1, "AAA")]
        [TestCase(2, "GME")]
        public async Task GetTickersByTickerId(int id, string expectedName)
        {

            var ret = await dao.GetTickerByTickerId(id);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedName, ret.Name);
            Assert.Pass();
        }

        [TestCase("AAA", 1)]
        [TestCase("GME", 2)]
        public async Task GetTickersByName(string name, int expectedId)
        {
            var ret = await dao.GetTickerByName(name);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedId, ret.Id);
            Assert.Pass();
        }

        [TestCase(1, new int[] { 1, 2 })]
        [TestCase(2, new int[] { 3, 4, 5 })]
        public async Task GetTickersByCompanyId(int id, int[] expectedTickerIds)
        {

            var ret = await dao.GetTickersByCompanyId(id);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedTickerIds.ToList(), ret.Select(t => t.Id));
            Assert.Pass();
        }

        [TestCase(1, new object[] { "AAA", "GME", "TCL" })]
        [TestCase(2, new object[] { "GOOGL", "GOOG" })]
        [TestCase(3, new object[] { "G", "D", "APL" })]
        public async Task UpdateCompanyTickers(int companyId, object[] tickerNames)
        {
            List<string> tickers = Array.ConvertAll(tickerNames, t => t.ToString()).ToList();

            var ret = await dao.UpdateCompanyTickers(companyId, tickers);

            Assert.NotNull(ret);
            Assert.AreEqual(tickers, ret.Select(t => t.Name));
            Assert.Pass();
        }
    }
}
