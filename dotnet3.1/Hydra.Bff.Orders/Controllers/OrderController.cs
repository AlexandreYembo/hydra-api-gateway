using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Bff.Orders.Models;
using Hydra.Bff.Orders.Services;
using Hydra.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Bff.Orders.Controllers
{
    public class OrderController : MainController
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IVoucherService _voucherService;

        public OrderController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService, ICustomerService customerService, IVoucherService voucherService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;
            _customerService = customerService;
            _voucherService = voucherService;
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> CreateOrder(OrderDTO order)
        {
            var basket = await _basketService.GetBasket();
            
            if(basket == null)
            {
                AddErrors("Basket is empty");
                return CustomResponse();
            }

            var voucher = await _voucherService.GetVoucherByCode(basket.Voucher?.Code);

            if(voucher == null)
            {
                AddErrors("Voucher is invalid");
                return CustomResponse();
            }

            var products = await _catalogService.GetItems(basket.Items.Select(i => i.ProductId));

            if(!await BasketItemsValidation(basket, products)) return CustomResponse();

            var address = await _customerService.GetAddress();

            if(address == null)
            {
                AddErrors("Address not found");
                return CustomResponse();
            }

            CreateOrderObject(basket, address, voucher, order);
            return CustomResponse(await _orderService.CreateOrder(order));
        }

        // [HttpGet]
        // [Route("order/latest")]
        // public async Task<IActionResult> GetLatest()
        // {
            
        // }

        // [ HttpGet]
        // [Route("order/customer")]
        // public async Task<IActionResult> GetByCustomer()
        // {
            
        // }

        private async Task<bool> BasketItemsValidation(BasketDTO basket, IEnumerable<CatalogItemDTO> products)
        {
            if(basket.Items.Count != products.Count())
            {
                var itemsNotAvailable = basket.Items.Select(bi => bi.ProductId)
                                                    .Except(products.Select(p => p.Id)).ToList();

                foreach (var itemId in itemsNotAvailable)
                {
                    var basketItem = basket.Items.FirstOrDefault(bi => bi.ProductId == itemId);
                    AddErrors($"The item{basketItem.Name} is not available in the catalog.");
                }

                return false;
            }

            foreach (var basketItem in basket.Items)
            {
                var catalogProduct = products.FirstOrDefault(p => p.Id == basketItem.ProductId);

                if(catalogProduct.Price != basketItem.Price)
                {
                    AddErrors($"The price of {basketItem.Name} changed (from {basketItem.Price} to {catalogProduct.Price})");

                    var deleteResponse = await _basketService.RemoveBasketItem(basketItem.ProductId);

                    if(ResponseHasErrors(deleteResponse))
                    {
                        AddErrors($"An error occured to remove the item from the basket, please if you want to buy this item, first remove from the basket and then add manually again!");
                        return false;
                    }

                    basketItem.Price = catalogProduct.Price;

                    var addResponse = await _basketService.AddBasketItem(basketItem);

                    if(ResponseHasErrors(addResponse))
                    {
                        AddErrors($"An error occured to update the item to the basket, please if you want to buy this item, first remove from the basket and then add manually again!");
                        return false;
                    }

                    ClearErrors();
                    AddErrors($"The price of {basketItem.Name} chanced (from {basketItem.Price} to {catalogProduct.Price}). The item of the basket has been updated.");
                    return false;
                }
            }
            return true;
        }

        private void CreateOrderObject(BasketDTO basket, AddressDTO address, VoucherDTO voucher, OrderDTO order)
        {
            order.Voucher = voucher;
            order.HasVoucher = basket.HasVoucher;
            order.TotalPrice = basket.TotalPrice;
            order.Items = basket.Items;
            order.Address = address;
        }
    }
}