using System.Net.Http;
using Hydra.Bff.Orders.Extensions;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IPaymentService
    {
    }

    public class PaymentService : Service, IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.PaymentUrl);
        }
    }
}