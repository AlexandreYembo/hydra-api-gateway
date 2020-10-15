using System.Threading.Tasks;
using Hydra.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hydra.Bff.Orders.Controllers
{
    [Authorize]
    public class BasketController : MainController
    {
        [HttpGet]
        [Route("order/basket")]
        public async Task<IActionResult> Get()
        {
            return CustomResponse();
        }

        [HttpPost]
        [Route("order/basket-quantity")]
        public async Task<IActionResult> GetBasketQuantity()
        {
            return CustomResponse();
        }

        [HttpPost]
        [Route("order/basket/items")]
        public async Task<IActionResult> AddBasketItems()
        {
            return CustomResponse();
        }

        [HttpPut]
        [Route("order/basket/items/{productId}")]
        public async Task<IActionResult> UpdateBasketItems()
        {
            return CustomResponse();
        }

        [HttpDelete]
        [Route("order/basket/items/{productId}")]
        public async Task<IActionResult> DeleteBasketItems()
        {
            return CustomResponse();
        }
    }
}