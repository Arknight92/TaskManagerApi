using TaskManagerApi.DTOs.Task;

namespace TaskManagerApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetUserTasksAsync(int userId);
        Task<TaskDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<bool> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int userId, int taskId);
    }
}
