namespace TodoTracker.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Login { get; set; } = string.Empty;
    
    public string PasswordHash { get;set; } = string.Empty;

    public string? Name { get; set; }

    public DateTime RegisteredAt { get;set; }
}
