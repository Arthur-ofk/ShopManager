using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DAL.Abstractions;

namespace DAL.Repos
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private string _connectionString;
        public ItemRepository(string connectionString)
            : base(connectionString, "Items")
        {
            _connectionString = connectionString;
        }


        public async Task<IEnumerable<Item>> GetItemsByCategoryAsync(Guid categoryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Items WHERE CategoryId = @CategoryId";
                return await connection.QueryAsync<Item>(sql, new { CategoryId = categoryId });
            }
        }

        public async Task UpdateQuantityAsync(Guid itemId, int quantityToSubtract)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
            UPDATE Items 
            SET Quantity = Quantity - @QuantityToSubtract 
            WHERE Id = @itemId AND Quantity >= @QuantityToSubtract";

                
                var rowsAffected = await connection.ExecuteAsync(sql, new { ItemId = itemId, QuantityToSubtract = quantityToSubtract });

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException("Insufficient quantity or item not found.");
                }
            }
        }
        public async Task<Item> CreateItemAsync(Item item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = $"INSERT INTO Items (Id ,Name, Description, Quantity, Price, CategoryId,CreatedAt,UpdatedAt) " +
                          $"OUTPUT INSERTED.* " +
                          $"VALUES (@Id,@Name, @Description, @Quantity, @Price, @CategoryId,@CreatedAt,@UpdatedAt)";

                
                var parameters = new
                {   
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CategoryId = item.CategoryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                return await connection.QuerySingleOrDefaultAsync<Item>(sql, parameters);
            }
        }
    }
}