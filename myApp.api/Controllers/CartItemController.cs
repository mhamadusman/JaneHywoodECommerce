using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace myApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private const string SessionKey = "CartItems";

        public CartItemController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddCartItem([FromBody] CartItem item)
        {
            await _cartRepository.addItem(item);

            var sessionItems = HttpContext.Session.GetString(SessionKey);
            List<CartItem> cartItems = string.IsNullOrEmpty(sessionItems)
                                        ? new List<CartItem>()
                                        : JsonSerializer.Deserialize<List<CartItem>>(sessionItems);

            cartItems.Add(item);

            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(cartItems));

            return CreatedAtAction(nameof(GetCartItemById), new { id = item.ProductId, userid = item.UserId }, item);
        }

        [HttpGet("json/{json}")]
        public async Task<ActionResult<List<CartItem>>> GetJsonItems(string json)
        {
            var sessionItems = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionItems))
            {
                return BadRequest("No items found in session.");
            }

            var items = JsonSerializer.Deserialize<List<CartItem>>(sessionItems);

            return Ok(items);
        }

        [HttpGet("count/{userid}")]
        public async Task<ActionResult<int>> GetItemCount(string userid)
        {
            int count = await _cartRepository.getItemCount(userid);
            return Ok(count);
        }

        [HttpGet("sessioncount")]
        public ActionResult<int> GetSessionItemCount()
        {
            var sessionItems = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionItems))
            {
                return Ok(0);
            }

            var items = JsonSerializer.Deserialize<List<CartItem>>(sessionItems);
            return Ok(items.Count);
        }

        [HttpGet("{id}/{userid}")]
        public async Task<ActionResult<CartItem>> GetCartItemById(int id, string userid)
        {
            var item = await _cartRepository.getItem(id, userid);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCartItem(int id, [FromBody] CartItem item)
        {
            if (id != item.ProductId)
            {
                return BadRequest();
            }

            await _cartRepository.Update(item);

            var sessionItems = HttpContext.Session.GetString(SessionKey);
            if (!string.IsNullOrEmpty(sessionItems))
            {
                var items = JsonSerializer.Deserialize<List<CartItem>>(sessionItems);
                var existingItem = items.FirstOrDefault(i => i.ProductId == id);
                if (existingItem != null)
                {
                    existingItem.Quantity = item.Quantity;
                    HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(items));
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}/{userid}")]
        public async Task<ActionResult> RemoveCartItem(int id, string userid)
        {
            var item = await _cartRepository.getItem(id, userid);
            if (item == null)
            {
                return NotFound();
            }

            await _cartRepository.removeItem(id, userid);

            var sessionItems = HttpContext.Session.GetString(SessionKey);
            if (!string.IsNullOrEmpty(sessionItems))
            {
                var items = JsonSerializer.Deserialize<List<CartItem>>(sessionItems);
                var itemToRemove = items.FirstOrDefault(i => i.ProductId == id);
                if (itemToRemove != null)
                {
                    items.Remove(itemToRemove);
                    HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(items));
                }
            }

            return NoContent();
        }

        [HttpGet("useritems/{userid}")]
        public async Task<ActionResult<List<CartItem>>> GetUserItems(string userid)
        {
            var items = await _cartRepository.getUserItems(userid);
            return Ok(items);
        }
    }

}

