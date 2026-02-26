using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs.Task
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = String.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
