using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemResponseDto>>> GetTodoItems(bool? isComplete)
        {
            var todoItems = await _context.TodoItems.ToListAsync();

            List<TodoItem> filteredItems = new List<TodoItem>();
            if (isComplete.HasValue)
            {
                foreach (var item in todoItems)
                {
                    if (item.IsComplete == isComplete.Value)
                    {
                        filteredItems.Add(item);
                    }
                }
            }
            else
            {
                // Include all items if no filter is provided
                filteredItems.AddRange(todoItems);
            }

            var response = filteredItems.Select(ItemToResponseDto);
            return Ok(response);
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemResponseDto>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToResponseDto(todoItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemRequestDto todoRequestDto)
        {
            if (id != todoRequestDto.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoRequestDto.Name;
            todoItem.IsComplete = todoRequestDto.IsComplete;

            // Set CompletedTime if IsComplete is set to true
            if (todoRequestDto.IsComplete)
            {
                todoItem.CompletedTime = DateTime.Now;
            }
            else
            {
                todoItem.CompletedTime = null; // Set to null if IsComplete is false
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItemResponseDto>> PostTodoItem(TodoItemRequestDto todoRequestDto)
        {
            DateTime currentTime = DateTime.Now;
            var todoItem = new TodoItem
            {
                IsComplete = todoRequestDto.IsComplete,
                Name = todoRequestDto.Name,
                CreatedTime = currentTime // Set the CreatedTime upon creation
            };

            // Set CompletedTime if IsComplete is true
            if (todoRequestDto.IsComplete)
            {
                todoItem.CompletedTime = currentTime;
            }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Return the created TodoItem including CreatedTime and CompletedTime
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToResponseDto(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        private static TodoItemResponseDto ItemToResponseDto(TodoItem todoItem) =>
           new TodoItemResponseDto
           {
               Id = todoItem.Id,
               Name = todoItem.Name,
               IsComplete = todoItem.IsComplete,
               CreatedTime = todoItem.CreatedTime,
               CompletedTime = todoItem.CompletedTime
           };
    }
}
