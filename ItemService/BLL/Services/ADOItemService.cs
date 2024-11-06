using BLL.Shared;
using DAL.Models;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ADOItemService
    {
        private readonly ADOItemRepository _repository;

        public ADOItemService(ADOItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<ItemDTO> CreateItemAsync(ItemDTO itemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                CategoryId = itemDto.CategoryId
            };

            var createdItem = await _repository.CreateAsync(item);
            return new ItemDTO
            {
                Id = createdItem.Id,
                Name = createdItem.Name,
                Description = createdItem.Description,
                Quantity = createdItem.Quantity,
                Price = createdItem.Price,
                CategoryId = createdItem.CategoryId
            };
        }

        public async Task<ItemDTO> GetItemByIdAsync(Guid id)
        {
            var item = await _repository.GetByIdAsync(id);
            return new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                CategoryId = item.CategoryId
            };
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {
            var items = await _repository.GetAllAsync();
            var itemDtos = new List<ItemDTO>();
            foreach (var item in items)
            {
                itemDtos.Add(new ItemDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    CategoryId = item.CategoryId
                });
            }
            return itemDtos;
        }

        public async Task UpdateItemAsync(ItemDTO itemDto)
        {
            var item = new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Description = itemDto.Description,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                CategoryId = itemDto.CategoryId
            };
            await _repository.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
