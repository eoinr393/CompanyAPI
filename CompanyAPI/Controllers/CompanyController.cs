using Company.DataAccess.Controllers;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompanyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyDAO companyDAO;
        private readonly ITickerDAO tickerDAO;
        private readonly IExchangeDAO exchangeDAO;
        private readonly ICompanyExchangeDAO companyExchangeDAO;

        public CompanyController(ILogger<CompanyController> logger, ICompanyDAO companyDAO, ITickerDAO tickerDAO, IExchangeDAO exchangeDAO, ICompanyExchangeDAO companyExchangeDAO)
        {
            _logger = logger;
            this.companyDAO = companyDAO;
            this.tickerDAO = tickerDAO;
            this.exchangeDAO = exchangeDAO;
            this.companyExchangeDAO = companyExchangeDAO;
        }

        /// <summary>
        /// Update a Company Object
        /// </summary>
        /// <param name="companyUpdate"></param>
        /// <returns>The Updated company</returns>
        [HttpPut()]
        public async Task<IActionResult> Update(CompanyModel companyUpdate)
        {
            if (companyUpdate == null)
                return BadRequest("Company not recieved");

            var existingCompany = await companyDAO.GetCompanyById(companyUpdate.Id);

            if (existingCompany.ISIN != companyUpdate.ISIN && !this.ValidateISIN(companyUpdate.ISIN))          
                return BadRequest("ISIN value must begin with two letters and be unique");
           

            //Update Company
            var updatedCompany = await companyDAO.UpdateCompany(new Company.DataAccess.Models.Company
            {
                Id = companyUpdate.Id,
                Name = companyUpdate.Name,
                ISIN = companyUpdate.ISIN,
                Website = companyUpdate.Website
            });


            //Create / check exchanges exist already
            var exchangeIds = new List<int>();

            foreach (string s in companyUpdate.CompanyExchange)
            {
                var newExchange = await exchangeDAO.CreateExchange(new Company.DataAccess.Models.Exchange
                {
                    Name = s
                });

                exchangeIds.Add(newExchange.Id);
            }

            //Update join table
            var updatedExchange = await companyExchangeDAO.UpdateCompanyExchanges(companyUpdate.Id, exchangeIds);

            //Create/Update Tickers
            var updatedTicker = await tickerDAO.UpdateCompanyTickers(companyUpdate.Id, companyUpdate.Ticker);

            if (updatedCompany != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error occured please try again.");
            }
        }

        /// <summary>
        /// Get All companies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get all company info
            var allCompanies = await companyDAO.GetCompanys();

            //Load up Ticker and Exchange data
            List<CompanyModel> companyInfo = new List<CompanyModel>();

            foreach (Company.DataAccess.Models.Company c in allCompanies)
            {

                var tickers = await tickerDAO.GetTickersByCompanyId(c.Id);
                var companyExchanges = await companyExchangeDAO.GetCompanyExchangesByCompanyId(c.Id);

                var exchanges = new List<Company.DataAccess.Models.Exchange>();

                foreach (var ce in companyExchanges)
                {
                    exchanges.Add(await exchangeDAO.GetExchangeById(ce.ExchangeId));
                }

                companyInfo.Add(CompanyMapper.DAOToAPIModel(c, exchanges, tickers));
            }

            if (companyInfo.Count != 0)
                return Ok(companyInfo);

            return NotFound("No company exists yet.");
        }

        /// <summary>
        /// Get a company by either ID or by ISIN
        /// </summary>
        /// <param name="id">the ID or the ISIN to be for searching the DB</param>
        /// <returns>The company object</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(string id)
        {
            int.TryParse(id, out int companyId);

            var company = companyId > 0 ? await companyDAO.GetCompanyById(companyId) : await companyDAO.GetCompanyByISIN(id);

            if (company == null)
                return NotFound($"no company found for id: {id}");

            var tickers = await tickerDAO.GetTickersByCompanyId(company.Id);
            var companyExchanges = await companyExchangeDAO.GetCompanyExchangesByCompanyId(company.Id);

            var exchanges = new List<Company.DataAccess.Models.Exchange>();

            foreach (var ce in companyExchanges)
            {
                exchanges.Add(await exchangeDAO.GetExchangeById(ce.ExchangeId));
            }

            var companyInfo = CompanyMapper.DAOToAPIModel(company, exchanges, tickers);

            return Ok(companyInfo);
        }

        /// <summary>
        /// Create a new Company
        /// </summary>
        /// <param name="company">The company to create</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CompanyModel company)
        {
            if (!this.ValidateISIN(company.ISIN))
                return BadRequest("ISIN value must begin with two letters and be unique");
           
            //Create Company
            var newCompany = await companyDAO.CreateCompany(new Company.DataAccess.Models.Company
            {
                Name = company.Name,
                ISIN = company.ISIN,
                Website = company.Website
            });

            //Create Tickers
            foreach (var ticker in company.Ticker)
            {
                await tickerDAO.CreateTicker(new Company.DataAccess.Models.Ticker { CompanyId = newCompany.Id, Name = ticker });
            }

            //Create Exchanges and link to Copmany in join table
            var exchanges = new List<Company.DataAccess.Models.Exchange>();

            foreach (var exchange in company.CompanyExchange)
            {
                var newExchange = await exchangeDAO.CreateExchange(new Company.DataAccess.Models.Exchange { Name = exchange });
                exchanges.Add(newExchange);
                await companyExchangeDAO.CreateCompanyExchange(new Company.DataAccess.Models.CompanyExchange { CompanyId = newCompany.Id, ExchangeId = newExchange.Id });
            }

            var companyInfo = CompanyMapper.DAOToAPIModel(newCompany, exchanges, newCompany.Ticker.ToList());

            return Ok(companyInfo);
        }

        /// <summary>
        /// Validates the ISIN
        /// </summary>
        /// <param name="isin">the ISIN to validate</param>
        /// <returns>Whether the isin is valid</returns>
        private bool ValidateISIN(string isin)
        {
            bool passedValidation = true;

            if (!Regex.Match(isin, "^[a-zA-Z][a-zA-Z]").Success || companyDAO.GetCompanyByISIN(isin).Result != null)
            {
                return false;
            }

            return passedValidation;
        }
    }
}
