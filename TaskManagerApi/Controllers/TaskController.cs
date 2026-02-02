using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private static List<TaskItem> _tasks = new List<TaskItem>();


        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetAll()
        {
            return Ok(_tasks);
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            task.Id = _tasks.Count + 1;
            task.CreateAt = DateTime.UtcNow;
            _tasks.Add(task);

            return CreatedAtAction(nameof(GetAll), new { id = task.Id }, task);
        }
    }
}
