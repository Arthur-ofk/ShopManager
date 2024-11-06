using BLL.Services;
using BLL.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ItemsService.Controllers
{
    [ApiController]
    [Route("api/pureado/items")]
    public class ADOItemsController : ControllerBase
    {
        private readonly ADOItemService _itemService;

        public ADOItemsController(ADOItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(Guid id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItem(ItemDTO itemDto)
        {
            var createdItem = await _itemService.CreateItemAsync(itemDto);
            return CreatedAtAction(nameof(GetItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, ItemDTO itemDto)
        {
            if (id != itemDto.Id)
            {
                return BadRequest();
            }

            await _itemService.UpdateItemAsync(itemDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}
