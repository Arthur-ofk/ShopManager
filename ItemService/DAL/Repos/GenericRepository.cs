using DAL.Abstractions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public GenericRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        public async Task<T> CreateAsync(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"INSERT INTO {_tableName} OUTPUT INSERTED.* VALUES (@entity)";
                return await connection.QuerySingleOrDefaultAsync<T>(sql, entity);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"UPDATE {_tableName} SET @entity WHERE Id = @Id";
                await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"SELECT * FROM {_tableName}";
                return await connection.QueryAsync<T>(sql);
            }
        }
    }
}
