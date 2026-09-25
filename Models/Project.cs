namespace TodoTracker.Models;

public class Project
{
    public User Owner { get; set; } = null!;
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
    public List<TodoTask> Tasks { get; set; } = new();
}