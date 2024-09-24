using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productService;

        public ProductService(IProductRepository productService)
        {
            _productService = productService;
        }

        public async Task Add(Product product)
        {
            await _productService.Add(product);
        }

        public async Task Update(Product product)
        {
            await _productService.Update(product);
        }

        public async Task delete(int id)
        {
            await _productService.delete(id);
        }

        public async Task<List<Product>> get(string catgry)
        {
            return await _productService.get(catgry);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productService.GetAll();
        }

        public async Task<Product> find(int productId)
        {
            return await _productService.find(productId);
        }

        public async Task<List<Product>> findProductByName(string q) // removed : [fromQury]
        {
            return await _productService.findProductByName(q);
        }

        public async Task addOrderProduct(OrderProduct op)
        {
            await _productService.addOrderProduct(op);
        }

        public async Task<int> getOrderId(string userId)
        {
            return await _productService.getOrderId(userId);
        }

        public async Task addCustomer(Customer c)
        {
            await _productService.addCustomer(c);
        }

        public async Task<List<OrderProduct>> GetOrders()
        {
            return await _productService.GetOrders();
        }

        public async Task<List<Customer>> GetCustomers(string adminId)
        {
            return await _productService.GetCustomers(adminId);
        }
        public async Task<int> getItemQuantity(int id)
        {
            return await _productService.getItemQuantity(id); ;
        }
    }
}
