using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace TaskManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        {
            var userId = GetUserId();
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            task.CreateAt = DateTime.UtcNow;
            task.UserId = GetUserId();

            _context.Tasks.Add(task);

            // change object into SQL, sennd it to database and get generated ID
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }

        [HttpPut("{id}")] 

        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            // FindAsync => finding task using primary key (id)
            var existingTask = await _context.Tasks.FindAsync(id);
            var userId = GetUserId();

            if(existingTask == null || existingTask.UserId != userId)
            {
                return NotFound();
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            await _context.SaveChangesAsync();

            return NoContent(); // Success, with no return value

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            var userId = GetUserId();

            if(task==null || task.UserId != userId) { return NotFound(); }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim.Value);
        }
    }
}
