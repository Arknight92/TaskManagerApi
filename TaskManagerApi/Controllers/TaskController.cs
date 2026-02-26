using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs.Task;
using TaskManagerApi.Models;
using TaskManagerApi.Services.Interfaces;


namespace TaskManagerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskResponseDto>>> Get()
        {
            return Ok(await _service.GetUserTasksAsync(GetUserId()));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create([FromBody] CreateTaskDto dto)
        {
            var userId = GetUserId();
            var created = await _service.CreateTaskAsync(userId, dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")] 

        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            var success = await _service.UpdateTaskAsync(GetUserId(), id, dto);
            return success ? NoContent() : NotFound();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteTaskAsync(GetUserId(), id);
            return success ? NoContent() : NotFound();
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return int.Parse(claim.Value);
        }
    }
}
