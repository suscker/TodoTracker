using System.ComponentModel.DataAnnotations;

namespace TodoTracker.Models.Dto;
public class RegisterUserRequest
{
    [Required] 
    [StringLength(50, MinimumLength = 3)] 
    public string Login { get; set; } = string.Empty;

    [Required] 
    [StringLength(100, MinimumLength = 8)] 
    public string Password {get; set;} = string.Empty;
    
    
    [StringLength(100)] 
    public string? Name {get;set;}

    


}