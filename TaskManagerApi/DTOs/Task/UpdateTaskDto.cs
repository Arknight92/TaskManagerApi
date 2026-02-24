using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs.Task
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = "";

        public bool IsCompleted { get; set; }
    }
}
