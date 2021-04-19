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
    /// <summary>
    /// Tests relating to ExchangeDAO
    /// </summary>
    [TestFixture]
    public class ExchangeDAOTest
    {
        private IExchangeDAO dao;

        private DbContextOptions<CompanyDBContext> options = new DbContextOptionsBuilder<CompanyDBContext>().UseInMemoryDatabase(databaseName: "ExchangeDatabase").Options;

        [SetUp]
        public void SetUp()
        {
            // Insert seed data into the database using one instance of the context
            using (var context = new CompanyDBContext(options))
            {
                context.Database.EnsureDeleted();

                context.Exchange.Add(new Exchange { Name = "New York Stock Exchange" });
                context.Exchange.Add(new Exchange { Name = "Nasdaq" });
                context.Exchange.Add(new Exchange { Name = "Japan Exchange Group" });
                context.Exchange.Add(new Exchange { Name = "London Stock Exchange" });
                context.Exchange.Add(new Exchange { Name = "Shanghai Stock Exchange" });
                context.Exchange.Add(new Exchange { Name = "Hong Kong Stock Exchange" });
                context.Exchange.Add(new Exchange { Name = "Euronext" });
                context.Exchange.Add(new Exchange { Name = "Toronto Stock Exchange" });

                context.SaveChanges();
            }
            
            dao = new ExchangeDAO(new CompanyDBContext(options));                          
        }

        [TestCase("Shenzhen Stock Exchange")]
        public async Task CreateExchange(string exchangeName)
        {
            var ret = await dao.CreateExchange(new Exchange { Name = exchangeName });

            Assert.NotNull(ret);
            Assert.AreEqual(exchangeName, ret.Name);
            Assert.Pass();
        }


        [TestCase(1, "New York Stock Exchange")]
        [TestCase(8, "Toronto Stock Exchange")]
        public async Task GetExchangeById(int id, string expectedName)
        {
            var ret = await dao.GetExchangeById(id);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedName, ret.Name);
            Assert.Pass();
        }

        [TestCase("Toronto Stock Exchange", 8)]
        [TestCase("New York Stock Exchange", 1)]
        [TestCase("Nasdaq", 2)]
        public async Task GetExchangeByName(string name, int expectedId)
        {
            var ret = await dao.GetExchangeByName(name);

            Assert.NotNull(ret);
            Assert.AreEqual(expectedId, ret.Id);
            Assert.Pass();
        }

    }
}
