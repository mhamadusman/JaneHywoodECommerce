using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace myApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")] 
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public OrderProductController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/OrderProduct
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderProduct>>> GetOrders()
        {
            var orders = await _cartRepository.GetOrders();
            return Ok(orders);
        }

        // POST: api/OrderProduct
        [HttpPost]
        public async Task<ActionResult> AddOrderProduct([FromBody] OrderProduct orderProduct)
        {
            if (orderProduct == null)
            {
                return BadRequest("OrderProduct cannot be null.");
            }

            await _cartRepository.addOrderProduct(orderProduct);
            return CreatedAtAction(nameof(GetOrders), new { id = orderProduct.ProductId }, orderProduct);
        }

        // Optional: Additional methods based on your application's needs.
    }
}
