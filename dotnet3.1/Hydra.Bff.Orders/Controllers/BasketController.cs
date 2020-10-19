using System;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Models;
using Hydra.Bff.Orders.Services;
using Hydra.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Bff.Orders.Controllers
{
    [Authorize]
    public class BasketController : MainController
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;

        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("order/basket")]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _basketService.GetBasket());
        }

        [HttpPost]
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

        private async Task BasketItemValidation(CatalogItemDTO product, int quantity)
        {
            if(product == null) AddErrors("Product does not exist");
            if(quantity < 1) AddErrors($"Chose at least 1 quantity of the product {product.Name}");

            var basket = await _basketService.GetBasket();
            var basketItem = basket.Items.FirstOrDefault(p => p.ProductId == product.Id);

            if(basketItem != null && basketItem.Qty + quantity > product.QtyStock)
            {
                AddErrors($"Product {product.Name} has {product.QtyStock} quantities available, you have selected {quantity} quantities");
                return;
            }

            if(quantity > product.QtyStock) AddErrors($"Product {product.Name} has {product.QtyStock} quantities available, you have selected {quantity} quantities");
        }
    }
}