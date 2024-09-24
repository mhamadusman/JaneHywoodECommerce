using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class CartService
    {
        private readonly ICartRepository _icart;

        public CartService(ICartRepository repo)
        {
            _icart = repo;
        }

        public async Task Add(CartItem cat)
        {
            await _icart.Add(cat);
        }

        public async Task<List<CartItem>> getJsonItems(string json)
        {
            return await _icart.getJsonItems(json);
        }

        public async Task<int> getItemCount(string userid)
        {
            return await _icart.getItemCount(userid);
        }

        public int getSessionItemCount(string s)
        {
            return  _icart.getSessionItemCount(s);
        }

        public async Task<CartItem> getItem(int id, string userid)
        {
            return await _icart.getItem(id, userid);
        }

        public async Task addItem(CartItem item)
        {
            await _icart.addItem(item);
        }

        public async Task Update(CartItem item)
        {
            await _icart.Update(item);
        }

        public async Task removeItem(int id, string userid)
        {
            await _icart.removeItem(id, userid);
        }
        public async Task deleteItemAfterCheckOut(string str)
        {
            await _icart.deleteItemAfterCheckOut(str);
        }
        public async Task<List<CartItem>> getUserItems(string userid)
        {
            return await _icart.getUserItems(userid);
        }

        public async Task addCustomer(Customer c)
        {
            await _icart.addCustomer(c);
        }

        public async Task<int> getOrderId(string userId)
        {
            return await _icart.getOrderId(userId);
        }

        public async Task addOrderProduct(OrderProduct op)
        {
            await _icart.addOrderProduct(op);
        }

        public async Task<List<Customer>> GetCustomers(string adminId)
        {
            return await _icart.GetCustomers(adminId);
        }
        public async Task<Customer> GetCustomerDetail(int orderId)
        {
            return await _icart.GetCustomerDetail(orderId);
        }
        public async Task<List<OrderProduct>> GetOrders()
        {
            return await _icart.GetOrders();
        }
        public async Task DeleteCustomer(string id)
        {
            await _icart.deleteCustomer(id);
        }
    }
}
