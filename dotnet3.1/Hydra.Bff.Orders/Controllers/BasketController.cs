using System;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Models;
using Hydra.Bff.Orders.Services;
using Hydra.Bff.Orders.Services.gRPC;
using Hydra.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Bff.Orders.Controllers
{
    [Authorize]
    public class BasketController : MainController
    {
        private readonly IBasketService _basketService;
        private readonly IBasketGrpcService _basketGrpcService;
        private readonly ICatalogService _catalogService;
        private readonly IVoucherService _voucherService;

        public BasketController(IBasketService basketService,
                                IBasketGrpcService basketGrpcService,
                                ICatalogService catalogService, 
                                IVoucherService voucherService)
        {
            _basketService = basketService;
            _basketGrpcService = basketGrpcService;
            _catalogService = catalogService;
            _voucherService = voucherService;
        }

        [HttpGet]
        [Route("order/basket")]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _basketService.GetBasket());
        }

        [HttpGet]
        [Route("order/gRPC/basket")]
        public async Task<IActionResult> GetgRPC()
        {
            return CustomResponse(await _basketGrpcService.GetBasket());
        }

        [HttpGet] 
        [Route("order/basket-quantity")]
        public async Task<int> GetBasketQuantity()
        {
            var basket = await _basketService.GetBasket();
            return basket?.Items.Sum(i => i.Qty) ?? 0;
        }

        [HttpPost]
        [Route("order/basket/items")]
        public async Task<IActionResult> AddBasketItems(BasketItemDTO basketItem)
        {
            var product = await _catalogService.GetById(basketItem.ProductId);

            await BasketItemValidation(product, basketItem.Qty);

            if(!ValidOperation()) return CustomResponse();

            basketItem.Name = product.Name;
            basketItem.Price = product.Price;
            basketItem.Image = product.Image;

            return CustomResponse(await _basketService.AddBasketItem(basketItem));
        }

        [HttpPut]
        [Route("order/basket/items/{productId}")]
        public async Task<IActionResult> UpdateBasketItems(Guid productId, BasketItemDTO basketItem)
        {
            var product = await _catalogService.GetById(productId);

            await BasketItemValidation(product, basketItem.Qty);

            if(!ValidOperation()) return CustomResponse();

            return CustomResponse(await _basketService.UpdateBasketItem(productId, basketItem));
        }

        [HttpDelete]
        [Route("order/basket/items/{productId}")]
        public async Task<IActionResult> DeleteBasketItems(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

           if(product == null)
           {
               AddErrors("Product does not exist");
               return CustomResponse();
           } 

            return CustomResponse(await _basketService.RemoveBasketItem(productId));
        }

        [HttpPost]
        [Route("order/basket/apply-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] string voucherCode)
        {
            var voucher = await _voucherService.GetVoucherByCode(voucherCode);

            if(voucher is null)
            {
                AddErrors("Invalid voucher");
                return CustomResponse();
            }

            var result = await _basketService.ApplyBasketVoucher(voucher);
            
            return CustomResponse(result);
        }

        private async Task BasketItemValidation(CatalogItemDTO product, int quantity)
        {
            if(product == null) AddErrors("Product does not exist");
            if(quantity < 1) AddErrors($"Chose at least 1 quantity of the product {product.Name}");

            var basket = await _basketService.GetBasket();
            var basketItem = basket.Items.FirstOrDefault(p => p.ProductId == product.Id);

            if(basketItem != null && basketItem.Qty + quantity > product.Qty)
            {
                AddErrors($"Product {product.Name} has {product.Qty} quantities available, you have selected {quantity} quantities");
                return;
            }

            if(quantity > product.Qty) AddErrors($"Product {product.Name} has {product.Qty} quantities available, you have selected {quantity} quantities");
        }
    }
}