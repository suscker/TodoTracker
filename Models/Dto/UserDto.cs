namespace TodoTracker.Models.Dto;

public class UserDto
{

    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;

    public string? Name {get; set;}
    
    public DateTime RegisteredAt {get;set;}

    


}