
using System.ComponentModel.DataAnnotations;

namespace TodoTracker.Models.Dto;

public class UpdateTodoTaskRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsDone { get; set; }

}