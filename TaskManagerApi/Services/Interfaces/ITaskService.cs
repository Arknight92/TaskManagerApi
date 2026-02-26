using TaskManagerApi.DTOs.Task;

namespace TaskManagerApi.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetUserTasksAsync(int userId);
        Task<TaskResponseDto> CreateTaskAsync(int userId, CreateTaskDto dto);
        Task<bool> UpdateTaskAsync(int userId, int taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(int userId, int taskId);
    }
}
