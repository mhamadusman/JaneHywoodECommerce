using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IProductRepository _productRepository;

        public ProductController(IRepository<Product> repository, IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Find(int id)
        {
            var product = await _repository.find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Product product)
        {
            await _repository.Add(product);
            return CreatedAtAction(nameof(Find), new { id = product.Id }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _repository.Edit(product);
            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _repository.find(id);
            if (product == null)
            {
                return NotFound();
            }

            await _repository.delete(id);
            return NoContent();
        }

        // GET: api/Product/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(string category)
        {
            var products = await _productRepository.get(category);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> FindProductByName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return BadRequest("Query string cannot be null or empty.");
            }

            var products = await _productRepository.findProductByName(productName);

            if (products == null || !products.Any())
            {
                return NotFound("No products found matching the search criteria.");
            }

            return Ok(products);
        }

    }
}
