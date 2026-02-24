using TaskManagerApi.Models;

namespace TaskManagerApi.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetByUserIdAsync(int userId);
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
        Task SaveChangesAsync();
    }
}
