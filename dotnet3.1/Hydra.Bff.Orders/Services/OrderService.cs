using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Extensions;
using Hydra.Bff.Orders.Models;
using Hydra.Core.Communication;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IOrderService
    {
        Task<ResponseResult> CreateOrder(OrderDTO order);
    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.OrderUrl);
        }

        public async Task<ResponseResult> CreateOrder(OrderDTO order)
        {
            var response = await _httpClient.PostAsync("/order/create", GetContent(order));

            if(!CheckResponseError(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

    }
}