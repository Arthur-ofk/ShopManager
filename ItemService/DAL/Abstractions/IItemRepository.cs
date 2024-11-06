using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstractions
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        
        Task<IEnumerable<Item>> GetItemsByCategoryAsync(Guid categoryId);
        Task UpdateQuantityAsync(Guid itemId, int quantityToSubtract);
        Task<Item> CreateItemAsync(Item item);
    }
}
