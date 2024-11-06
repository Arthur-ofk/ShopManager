using AutoMapper;
using BLL.Abstractions;
using BLL.Shared;
using DAL.Abstractions;
using DAL.Models;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ItemDTO> CreateItemAsync(ItemDTO itemDto)
        {
          
            var item = _mapper.Map<Item>(itemDto);
            var createdItem = await _itemRepository.CreateItemAsync(item);

            
            return _mapper.Map<ItemDTO>(createdItem);
        }

        public async Task<ItemDTO> GetItemByIdAsync(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task UpdateItemAsync(ItemDTO itemDto)
        {
            var item = _mapper.Map<Item>(itemDto);
            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _itemRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }
    }
}

