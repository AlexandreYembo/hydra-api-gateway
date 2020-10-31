using System;
using System.Collections.Generic;
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
        Task<IEnumerable<CatalogItemDTO>> GetItems(IEnumerable<Guid> ids);
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
            var response = await _httpClient.GetAsync($"/product/{id}");

            CheckResponseError(response);

            return await DeserializeResponseObject<CatalogItemDTO>(response);
        }

        public async Task<IEnumerable<CatalogItemDTO>> GetItems(IEnumerable<Guid> ids)
        {
            var request = string.Join(",", ids);
            var response = await _httpClient.GetAsync($"/product/list/{request}");

            CheckResponseError(response);

            return await DeserializeResponseObject<IEnumerable<CatalogItemDTO>>(response);
        }
    }
}