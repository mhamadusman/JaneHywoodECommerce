using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        public Task Add(Product product);
        public Task Update(Product product);
        public Task delete(int id);
        public Task<List<Product>> get(string catgry);
        public Task<List<Product>> GetAll();
        public Task<Product> find(int productId);
        public Task<List<Product>> findProductByName([FromQuery] string q);
        public Task addOrderProduct(OrderProduct op);
        public Task<int> getOrderId(string userId);
        public Task addCustomer(Customer c);
        public Task<List<OrderProduct>> GetOrders();
        public Task<List<Customer>> GetCustomers(string adminId);
        public Task<int> getItemQuantity(int id);

    }

    internal class FromQueryAttribute : Attribute
    {
    }
}
