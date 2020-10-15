using System.Net.Http;
using Hydra.Bff.Orders.Extensions;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IBasketService
    {
    }

    public class BasketService : Service, IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.BasketUrl);
        }
    }
}