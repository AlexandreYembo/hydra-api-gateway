using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Extensions;
using Hydra.Bff.Orders.Models;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetById(Guid id);
        Task<AddressDTO> GetAddress();
    }
    public class CustomerService : Service, ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.CustomerUrl);
        }

        public async Task<AddressDTO> GetAddress()
        {
            var response = await _httpClient.GetAsync($"/customer/address");

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
            
            CheckResponseError(response);

            return await DeserializeResponseObject<AddressDTO>(response);
        }

        public async Task<CustomerDTO> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/customer/{id}");

            CheckResponseError(response);

            return await DeserializeResponseObject<CustomerDTO>(response);
        }
    }
}