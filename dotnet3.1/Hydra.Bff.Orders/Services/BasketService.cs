using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Extensions;
using Hydra.Bff.Orders.Models;
using Hydra.Core.Communication;
using Microsoft.Extensions.Options;

namespace Hydra.Bff.Orders.Services
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasket();
        Task<ResponseResult> AddBasketItem(BasketItemDTO item);
        Task<ResponseResult> UpdateBasketItem(Guid productId, BasketItemDTO item);
        Task<ResponseResult> RemoveBasketItem(Guid productId);

        Task<ResponseResult> ApplyBasketVoucher(VoucherDTO voucher);
    }

    public class BasketService : Service, IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri(settings.Value.BasketUrl);
        }

        public async Task<ResponseResult> AddBasketItem(BasketItemDTO item)
        {
            var response = await _httpClient.PostAsync("/basket/", GetContent(item));

            if(!CheckResponseError(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateBasketItem(Guid productId, BasketItemDTO item)
        {
            var response = await _httpClient.PutAsync($"/basket/{productId}", GetContent(item));

            if(!CheckResponseError(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<BasketDTO> GetBasket()
        {
            var response = await _httpClient.GetAsync("/basket/");
            CheckResponseError(response);

            return await DeserializeResponseObject<BasketDTO>(response);
        }

        public async Task<ResponseResult> RemoveBasketItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/basket/{productId}");

            if(!CheckResponseError(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ApplyBasketVoucher(VoucherDTO voucher)
        {
            var response = await _httpClient.PostAsync("/basket/apply-voucher", GetContent(voucher));
            
            if(!CheckResponseError(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return ReturnOk();
        }
    }
}