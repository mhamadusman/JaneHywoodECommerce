using Core.Entities;
using Core.Interfaces;
using Inftastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepository<Customer> _repository;
        private readonly ICartRepository _cartRepository;

        public CustomerController(IRepository<Customer> repository, ICartRepository cartRepository)
        {
            _repository = repository;
            _cartRepository = cartRepository;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            var customers = await _repository.GetAll();
            return Ok(customers);
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerDetail(int id)
        {
            var customer = await _cartRepository.GetCustomerDetail(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Customer customer)
        {
            await  _cartRepository.addCustomer(customer);
            return CreatedAtAction(nameof(GetCustomerDetail), new { id = customer.OrderId }, customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            if (id ==0 )
            {
                Console.WriteLine("ID is empty or null");
                return BadRequest("ID cannot be empty");
            }

            Console.WriteLine("Received ID: " + id);

            var customer = await _cartRepository.GetCustomerDetail(id);
            if (customer == null)
            {
                return NotFound();
            }
            Console.WriteLine("Received ID: " + customer.OrderId);

            await _cartRepository.deleteCustomer(customer.Email);
            Console.WriteLine("Received ID: " + id);


            return NoContent();
        }

        [HttpGet("admin/{adminId}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string adminId)
        {
            var customers = await _cartRepository.GetCustomers(adminId);
            return Ok(customers);
        }

    }
}
