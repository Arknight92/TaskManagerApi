using TaskManagerApi.DTOs.Task;
using TaskManagerApi.Models;
using TaskManagerApi.Repositories.Interfaces;
using TaskManagerApi.Services.Interfaces;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TaskResponseDto>> GetUserTasksAsync(int userId)
        {
            var tasks = await _repo.GetByUserIdAsync(userId);

            return tasks.Select(ToResponseDto).ToList();
        }

        public async Task<TaskResponseDto> CreateTaskAsync(int userId, CreateTaskDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = false,
                CreateAt = DateTime.UtcNow,
                UserId = userId
            };

            await _repo.AddAsync(task);
            await _repo.SaveChangesAsync();

            return ToResponseDto(task);
        }

        public async Task<bool> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId) return false;

            task.Title = dto.Title.Trim();
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            _repo.Update(task);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int userId, int taskId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId) return false;

            _repo.Delete(task);
            await _repo.SaveChangesAsync();

            return true;
        }

        private static TaskResponseDto ToResponseDto(TaskItem t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreateAt
        };
    }
}