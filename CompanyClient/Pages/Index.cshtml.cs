using CompanyClient.API;
using CompanyClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CompanyClient.Pages
{
    /// <summary>
    /// Main Page to view and manipulate Companies
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<CompanyModel> CompanyModels = new List<CompanyModel>();

        public CompanyModel LoadedModel;

        private CompanyAPIConnector api;

        public IndexModel(IHttpClientFactory factory, ILogger<IndexModel> logger)
        {
            _logger = logger;
            this.api = new CompanyAPIConnector(factory);
        }

        /// <summary>
        /// Load all compnies into the model
        /// </summary>
        public void OnGet()
        {
            var result = this.api.GetAll();
            this.CompanyModels = result.Result;
        }

        /// <summary>
        /// Gets a single company by its isin and returns it as JSON
        /// </summary>
        /// <param name="isin">the isin of the company</param>
        /// <returns>copmanyModel object as json</returns>
        public JsonResult OnGetCompany(string isin)
        {
            var result = this.api.GetCompany(isin);
            var recievedModel = result.Result;

            CompanyModel viewModel = new CompanyModel
            {
                Id = recievedModel.Id,
                Name = recievedModel.Name,
                ISIN = recievedModel.ISIN,
                Website = recievedModel.Website,
                Ticker = new List<string> { String.Join(",", recievedModel.Ticker) },
                CompanyExchange = new List<string> { String.Join(",", recievedModel.CompanyExchange) }
            };

            return new JsonResult(JsonConvert.SerializeObject(viewModel));
        }

        // Model that gets loaded with the created company 
        [BindProperty]
        public Models.CompanyModel CreatedCompanyModel { get; set; }

        /// <summary>
        /// Creates a company
        /// </summary>
        /// <returns>Refreshes page</returns>
        public async Task<ActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CompanyModel apiCompanyModel = new CompanyModel
            {
                Name = CreatedCompanyModel.Name,
                ISIN = CreatedCompanyModel.ISIN,
                Website = CreatedCompanyModel.Website,
                Ticker = CreatedCompanyModel.Ticker[0].Replace("\n", "").Split(",").ToList(),
                CompanyExchange = CreatedCompanyModel.CompanyExchange[0].Replace("\n", "").Split(",").ToList()
            };

            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(apiCompanyModel), Encoding.UTF8, "application/json");
            await this.api.CreateCompany(contentPost);

            return RedirectToPage("Index");
        }

        // Model that gets loaded with the company to be updated
        [BindProperty]
        public Models.CompanyModel UpdatedCompanyModel { get; set; }
        /// <summary>
        /// Updates a company Model
        /// </summary>
        /// <returns>Refreshes page</returns>
        public async Task<ActionResult> OnPostUpdate()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CompanyModel apiCompanyModel = new CompanyModel
            {
                Id = UpdatedCompanyModel.Id,
                Name = UpdatedCompanyModel.Name,
                ISIN = UpdatedCompanyModel.ISIN,
                Website = UpdatedCompanyModel.Website,
                Ticker = UpdatedCompanyModel.Ticker[0].Replace("\n", "").Split(",").ToList(),
                CompanyExchange = UpdatedCompanyModel.CompanyExchange[0].Replace("\n", "").Split(",").ToList()
            };

            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(apiCompanyModel), Encoding.UTF8, "application/json");
            await this.api.UpdateCompany(contentPost);

            return RedirectToPage("Index");
        }
    }
}
