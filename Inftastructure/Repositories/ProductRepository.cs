using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inftastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDb;Integrated Security=True;";
        List<Product> products = new List<Product>();

        public async Task Add(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                string query = $"INSERT INTO Products(Name, Description, Price, Img, Quantity, Category) VALUES (@name, @description, @price, @images, @quantity, @category)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@images", product.Img);
                    cmd.Parameters.AddWithValue("@category", product.Category);
                    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Update(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                string query = "UPDATE Product SET Name = @name, Description = @description, Price = @price, Img = @images, Category = @category, Quantity = @quantity WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@images", product.Img);
                    cmd.Parameters.AddWithValue("@category", product.Category);
                    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@id", product.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                string query = $"DELETE FROM Product WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<int> getItemQuantity(int id)
        {
            string query = "SELECT Quantity FROM Product WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    object result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : throw new Exception("Item not found or quantity is invalid");
                }
            }
        }

        public async Task<List<Product>> get(string catgry)
        {
            string query = $"SELECT * FROM Product WHERE Category = @cat";
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@cat", catgry);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader[2]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Img = reader["Img"].ToString(),
                                Category = reader["Category"].ToString()
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public async Task<List<Product>> GetAll()
        {
            string query = "SELECT * FROM Product";
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader[2]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Img = reader["Img"].ToString(),
                                Category = reader["Category"].ToString()
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public async Task<Product> find(int productId)
        {
            Product p = null;
            string query = $"SELECT * FROM Product WHERE Id = @productId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            p = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToDecimal(reader[2]),
                                Description = reader["Description"].ToString(),
                                Img = reader["Img"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Category = reader["Category"].ToString()
                            };
                        }
                    }
                }
            }
            return p;
        }

        public async Task<List<Product>> findProductByName(string q)
        {
            List<Product> products = new List<Product>();
            string query = $"SELECT * FROM Product WHERE Name = @query";
            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@query", q);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Product product = new Product
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader[2]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Img = reader["Img"].ToString(),
                                Category = reader["Category"].ToString()
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public async Task addOrderProduct(OrderProduct op)
        {
            string query = $"INSERT INTO OrderProduct (OrderId, ProductId, Price, Quantity, Size) VALUES (@OrderId, @ProductId, @Price, @Quantity, @Size)";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderId", op.OrderId);
                    cmd.Parameters.AddWithValue("@ProductId", op.ProductId);
                    cmd.Parameters.AddWithValue("@Price", op.Price);
                    cmd.Parameters.AddWithValue("@Quantity", op.Quantity);
                    cmd.Parameters.AddWithValue("@Size", op.ProductSize);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> getOrderId(string userId)
        {
            int id = 0;
            string query = $"SELECT OrderId FROM Customer WHERE Email = @userId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            id = Convert.ToInt32(reader["OrderId"]);
                        }
                    }
                }
            }
            return id;
        }

        public async Task addCustomer(Customer c)
        {
            string query = "INSERT INTO Customer (FirstName, LastName, Address, City, ZipCode, Email, PhoneNumber) VALUES (@FirstName, @LastName, @Address, @City, @ZipCode, @Email, @PhoneNumber)";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, connection))
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

        public async Task<List<OrderProduct>> GetOrders()
        {
            List<OrderProduct> orders = new List<OrderProduct>();
            string query = "SELECT * FROM OrderProduct";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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
                                ProductSize = Convert.ToString(reader["Size"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public async Task<List<Customer>> GetCustomers(string adminId)
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT * FROM Customer WHERE Email != @adminId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@adminId", adminId);
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
    }
}
