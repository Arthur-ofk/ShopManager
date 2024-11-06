using BLL.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
            Task DeleteItemAsync(Guid id);
            Task UpdateItemAsync(ItemDTO itemDto);
        Task<ItemDTO> CreateItemAsync(ItemDTO itemDto);
        Task<ItemDTO> GetItemByIdAsync(Guid id);
    }
}
