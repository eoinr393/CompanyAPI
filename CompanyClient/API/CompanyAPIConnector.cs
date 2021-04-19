using CompanyClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompanyClient.API
{
    /// <summary>
    /// API connector for the Company API
    /// </summary>
    public class CompanyAPIConnector
    {
        private IHttpClientFactory httpClientFactory;
        private string apiAddress; 
        public CompanyAPIConnector(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Gets all Companies
        /// </summary>
        /// <returns>A list of companies</returns>
        public async Task<List<CompanyModel>> GetAll()
        {
            try
            {
                var client = this.httpClientFactory.CreateClient("companyApi");

                var response = await client.GetAsync(this.apiAddress);

                if (response.IsSuccessStatusCode)
                {
                    string jsonresult = await response.Content.ReadAsStringAsync();
                    List<CompanyModel> results = JsonConvert.DeserializeObject<List<CompanyModel>>(jsonresult);

                    return results;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new List<CompanyModel>();
        }

        /// <summary>
        /// Get a company by ISIN
        /// </summary>
        /// <param name="isin">the isin of the company</param>
        /// <returns>the company</returns>
        public async Task<CompanyModel> GetCompany(string isin)
        {
            try
            {
                var client = this.httpClientFactory.CreateClient("companyApi");

                var response = await client.GetAsync(isin);

                if (response.IsSuccessStatusCode)
                {
                    string jsonresult = await response.Content.ReadAsStringAsync();
                    CompanyModel results = JsonConvert.DeserializeObject<CompanyModel>(jsonresult);

                    return results;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new CompanyModel();
        }

        /// <summary>
        /// Creates a company
        /// </summary>
        /// <param name="content">The httpcontent containing the company to be created</param>
        /// <returns>Whether the company was created or not</returns>
        public async Task<bool> CreateCompany(HttpContent content)
        {
            try
            {
                var client = this.httpClientFactory.CreateClient("companyApi");

                var response = await client.PostAsync(this.apiAddress, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        /// <summary>
        /// Updates a company
        /// </summary>
        /// <param name="content">The httpcontent containing the copmany to be updated</param>
        /// <returns>whether the company was updated or not</returns>
        public async Task<bool> UpdateCompany(HttpContent content)
        {
            try
            {
                var client = this.httpClientFactory.CreateClient("companyApi");

                var response = await client.PutAsync(this.apiAddress, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }
    }
}
