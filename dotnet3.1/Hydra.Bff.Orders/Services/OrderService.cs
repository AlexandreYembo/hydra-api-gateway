using System.Net.Http;
using Hydra.Bff.Orders.Extensions;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IOrderService
    {
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.OrderUrl);
        }
    }
}