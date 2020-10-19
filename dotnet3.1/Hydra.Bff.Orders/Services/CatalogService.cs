using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Extensions;
using Hydra.Bff.Orders.Models;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface ICatalogService
    {
        Task<CatalogItemDTO> GetById(Guid id);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.CatalogUrl);
        }

        public async Task<CatalogItemDTO> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            CheckResponseError(response);

            return await DeserializeResponseObject<CatalogItemDTO>(response);
        }
    }
}