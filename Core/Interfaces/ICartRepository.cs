using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICartRepository
    {
        Task Add(CartItem cat);

        Task<List<CartItem>> getJsonItems(string json);

        Task<int> getItemCount(string userid);

        int getSessionItemCount(string s);

        Task<CartItem> getItem(int id, string userid);

        Task addItem(CartItem item);

        Task Update(CartItem item);

        Task removeItem(int id, string userid);

        Task deleteItemAfterCheckOut(string str);

        Task<List<CartItem>> getUserItems(string userid);

        Task addCustomer(Customer c);

        Task<int> getOrderId(string userId);

        Task addOrderProduct(OrderProduct op);

        Task<List<Customer>> GetCustomers(string adminId);
        Task<Customer> GetCustomerDetail(int orderId);
        Task deleteCustomer(string id);
        Task<List<OrderProduct>> GetOrders();
    }
}
