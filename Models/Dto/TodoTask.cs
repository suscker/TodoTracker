

namespace TodoTracker.Models.Dto;

public class TodoTaskDto
{
    public Guid Id {get; set; }

    public string Name {get; set; } = string.Empty;

    public string? Description {get; set; }

    public bool IsDone {get; set; }

    public Guid ProjectId {get; set; }



} 