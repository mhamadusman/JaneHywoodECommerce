using Core.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inftastructure.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
    {
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDb;Integrated Security=True;";

        public async Task Add(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");
            var columnNames = string.Join(",", properties.Select(x => x.Name));
            var parameterNames = string.Join(",", properties.Select(y => "@" + y.Name));
            var query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task delete(int id)
        {
            var tableName = typeof(TEntity).Name;
            var primaryKey = "Id";
            var q = $"DELETE FROM {tableName} WHERE {primaryKey} = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(q, new { Id = id });
            }
        }

        public async Task Edit(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");
            var columnUpdates = string.Join(",", properties.Select(x => $"{x.Name} = @{x.Name}"));

            var query = $"UPDATE {tableName} SET {columnUpdates} WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var tableName = typeof(TEntity).Name;
            var q = $"SELECT * FROM {tableName}";
            using (var connection = new SqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<TEntity>(q)).ToList();
            }
        }

        public async Task<TEntity> find(int id)
        {
            var tableName = typeof(TEntity).Name;
            var primaryKey = "Id";
            var query = $"SELECT * FROM {tableName} WHERE {primaryKey} = @Id;";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<TEntity>(query, new { Id = id });
            }
        }
    }
}
