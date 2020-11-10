using System;
using System.Threading.Tasks;
using Hydra.Basket.API.Services.gRPC;
using Hydra.Bff.Orders.Models;
using BasketgRPC = Hydra.Basket.API.Services.gRPC.Basket;

namespace Hydra.Bff.Orders.Services.gRPC
{
    public interface IBasketGrpcService
    {
         Task<BasketDTO> GetBasket();
    }
    
    public class BasketGrpcService : IBasketGrpcService
    {
        private readonly BasketgRPC.BasketClient _basketClient;

        public BasketGrpcService(BasketgRPC.BasketClient basketClient)
        {
            _basketClient = basketClient;
        }

        public async Task<BasketDTO> GetBasket()
        {
            var response = await _basketClient.GetBasketAsync(new Basket.API.Services.gRPC.GetBasketRequest());
            return MapResponse(response);
        }

         private BasketDTO MapResponse(BasketCustomerResponse basketResponse)
        {
            var basketDto = new BasketDTO
            {
                Discount = (decimal)basketResponse.Discount,
                HasVoucher = basketResponse.Hasvoucher
            };

            if(basketResponse.Voucher != null)
            {
                basketDto.Voucher = new VoucherDTO
                {
                    Code = basketResponse.Voucher.Code,
                    Discount = (decimal?)basketResponse.Voucher.Discount ?? 0,
                    DiscountType = basketResponse.Voucher.Discounttype
                };
            }

            foreach (var item in basketResponse.Items)
            {
                basketDto.Items.Add(new BasketItemDTO
                {
                    Name = item.Name,
                    Image = item.Image,
                    ProductId = Guid.Parse(item.Productdd),
                    Qty = item.Qty,
                    Price = (decimal)item.Price
                });
            }

            return basketDto;
        }
    }
}