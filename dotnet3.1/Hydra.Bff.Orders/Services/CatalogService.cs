using System.Net.Http;
using Hydra.Bff.Orders.Extensions;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface ICatalogService
    {
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.CatalogUrl);
        }
    }
}