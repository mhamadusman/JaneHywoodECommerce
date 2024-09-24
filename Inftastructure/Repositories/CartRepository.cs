using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Core.Entities;
using Core.Interfaces;
using System.Data.SqlClient;

namespace Inftastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDb;Integrated Security=True;";

        public async Task Add(CartItem cat)
        {
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = "INSERT INTO CartItem (Id, userId, Name, Description, Price, Category, Img, Quantity, DiscountPrice, Size) " +
                           "VALUES (@id, @userId, @name, @description, @price, @category, @img, @quantity, @discountPrice, @size)";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", cat.ProductId);
                    cmd.Parameters.AddWithValue("@userId", cat.UserId);
                    cmd.Parameters.AddWithValue("@name", cat.ProductName);
                    cmd.Parameters.AddWithValue("@description", cat.ProductDescription);
                    cmd.Parameters.AddWithValue("@price", cat.ProductPrice);
                    cmd.Parameters.AddWithValue("@category", cat.Category);
                    cmd.Parameters.AddWithValue("@img", cat.Img);
                    cmd.Parameters.AddWithValue("@quantity", cat.Quantity);
                    cmd.Parameters.AddWithValue("@discountPrice", cat.DiscPrice);
                    cmd.Parameters.AddWithValue("@size", cat.ProductSize);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<CartItem>> getJsonItems(string json)
        {
            return JsonSerializer.Deserialize<List<CartItem>>(json);
        }

        public async Task<int> getItemCount(string userid)
        {
            int count = 0;
            using (var connection = new SqlConnection(connect))
            {
                string q = $"SELECT Quantity FROM CartItem WHERE userId = @userid";
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(q, connection))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            count += Convert.ToInt32(reader["Quantity"]);
                        }
                    }
                }
            }
            return count;
        }

        public int getSessionItemCount(string s)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(s))
            {
                List<CartItem> items = getJsonItems(s).Result; // Note: Blocking call
                foreach (var item in items)
                {
                    count += item.Quantity;
                }
            }
            return count;
        }

        public async Task<CartItem> getItem(int id, string userid)
        {
            CartItem item = null;
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = $"SELECT * FROM CartItem WHERE id = @id AND userId = @userid";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            item = new CartItem
                            {
                                ProductId = Convert.ToInt32(reader[0]),
                                UserId = reader["userId"].ToString(),
                                ProductName = reader["Name"].ToString(),
                                ProductPrice = Convert.ToInt32(reader["Price"]),
                                DiscPrice = Convert.ToInt32(reader["DiscountPrice"]),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            };
                        }
                    }
                }
            }
            return item;
        }

        public async Task addItem(CartItem item)
        {
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = "INSERT INTO CartItem (Id, userId, Name, Description, Price, Category, Img, Quantity, DiscountPrice, Size) " +
                           "VALUES (@id, @userId, @name, @description, @price, @category, @img, @quantity, @discountPrice, @size)";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", item.ProductId);
                    cmd.Parameters.AddWithValue("@userId", item.UserId);
                    cmd.Parameters.AddWithValue("@name", item.ProductName);
                    cmd.Parameters.AddWithValue("@description", item.ProductDescription);
                    cmd.Parameters.AddWithValue("@price", item.ProductPrice);
                    cmd.Parameters.AddWithValue("@category", item.Category);
                    cmd.Parameters.AddWithValue("@img", item.Img);
                    cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                    cmd.Parameters.AddWithValue("@discountPrice", item.DiscPrice);
                    cmd.Parameters.AddWithValue("@size", item.ProductSize);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(CartItem item)
        {
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = "UPDATE CartItem SET Quantity = @quantity WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", item.ProductId);
                    cmd.Parameters.AddWithValue("@quantity", item.Quantity);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task removeItem(int id, string userid)
        {
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = $"DELETE FROM CartItem WHERE Id = @id AND UserId = @userid";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task deleteItemAfterCheckOut(string userid)
        {
            Console.WriteLine($"user id from cartRepose {userid}");
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = $"DELETE FROM CartItem WHERE userId = @userid";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<CartItem>> getUserItems(string userid)
        {
            List<CartItem> items = new List<CartItem>();
            string q = $"SELECT * FROM CartItem WHERE UserId = @userid";
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CartItem item = new CartItem
                            {
                                ProductId = Convert.ToInt32(reader[0]),
                                UserId = reader[1].ToString(),
                                ProductName = reader[2].ToString(),
                                ProductDescription = reader[3].ToString(),
                                ProductPrice = Convert.ToInt32(reader[4]),
                                Category = reader[5].ToString(),
                                Img = reader[6].ToString(),
                                Quantity = Convert.ToInt32(reader[7]),
                                DiscPrice = Convert.ToInt32(reader[8])
                            };
                            items.Add(item);
                        }
                    }
                }
            }
            return items;
        }

        public async Task addCustomer(Customer c)
        {
            string q = "INSERT INTO Customer (FirstName, LastName, Address, City, ZipCode, Email, PhoneNumber) " +
                       "VALUES (@FirstName, @LastName, @Address, @City, @ZipCode, @Email, @PhoneNumber)";
            using (var connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(q, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", c.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", c.LastName);
                    cmd.Parameters.AddWithValue("@Address", c.Address);
                    cmd.Parameters.AddWithValue("@City", c.City);
                    cmd.Parameters.AddWithValue("@ZipCode", c.ZipCode);
                    cmd.Parameters.AddWithValue("@Email", c.Email);
                    cmd.Parameters.AddWithValue("@PhoneNumber", c.PhoneNumber);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> getOrderId(string userId)
        {
            int id = 0;
            string q = $"SELECT OrderId FROM Customer WHERE Email = @userId";
            using (var connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(q, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            id = Convert.ToInt32(reader["OrderId"]);
                        }
                    }
                }
            }
            return id;
        }

        public async Task addOrderProduct(OrderProduct op)
        {
            string q = "INSERT INTO OrderProduct (OrderId, ProductId, Price, Quantity) " +
                       "VALUES (@OrderId, @ProductId, @Price, @Quantity)";
            using (var connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(q, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", op.OrderId);
                    cmd.Parameters.AddWithValue("@ProductId", op.ProductId);
                    cmd.Parameters.AddWithValue("@Price", op.Price);
                    cmd.Parameters.AddWithValue("@Quantity", op.Quantity);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<Customer> GetCustomerDetail(int orderId)
        {
            string query = "SELECT * FROM Customer WHERE OrderId = @orderId";
            using (var connection = new SqlConnection(connect))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Customer
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))

                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<List<Customer>> GetCustomers(string adminId)
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT * FROM Customer WHERE Email != @adminId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@adminId", adminId);
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Customer customer = new Customer
                            {
                                OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                ZipCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }
        public async Task deleteCustomer(string id)
        {
            Console.WriteLine("inside repo : ", id);
            using (SqlConnection con = new SqlConnection(connect))
            {
                await con.OpenAsync();
                string q = $"DELETE FROM Customer WHERE Email = @id";
                using (SqlCommand cmd = new SqlCommand(q, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<List<OrderProduct>> GetOrders()
        {
            List<OrderProduct> orders = new List<OrderProduct>();
            string query = "SELECT * FROM OrderProduct";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            OrderProduct order = new OrderProduct
                            {
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                ProductSize = Convert.ToString(reader[4])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

    }
}
