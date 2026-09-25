namespace TodoTracker.Models;

public class User
{
    public List<Project> Projects { get; set; } = new();
    public Guid Id { get; set; }
    
    public string Login { get; set; } = string.Empty;
    
    public string PasswordHash { get;set; } = string.Empty;

    public string? Name { get; set; }

    public DateTime RegisteredAt { get;set; }

    internal static ReadOnlySpan<byte> FindFirst(object nameIdentifier)
    {
        throw new NotImplementedException();
    }
}
