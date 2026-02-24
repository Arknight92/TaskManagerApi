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
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetUserTasksAsync(GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var result = await _service.CreateTaskAsync(GetUserId(), dto);
            return Ok(result);
        }

        [HttpPut("{id}")] 

        public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
        {
            var success = await _service.UpdateTaskAsync(GetUserId(), id, dto);
            return success ? NoContent() : NotFound();
        }


        [HttpDelete("{id}")]
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
