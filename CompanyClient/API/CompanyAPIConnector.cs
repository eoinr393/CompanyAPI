using CompanyClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CompanyClient.API
{
    public class CompanyAPIConnector
    {
        private IHttpClientFactory httpClientFactory;
        private string apiAddress; 
        public CompanyAPIConnector(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

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
                //Logging tba
            }

            return new List<CompanyModel>();
        }

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
                //Logging tba
            }

            return new CompanyModel();
        }

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
                //Logging tba
            }

            return false;
        }

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
                //Logging tba
            }

            return false;
        }
    }
}
