using System.ComponentModel.DataAnnotations;

namespace TodoTracker.Models.Dto;

public class LoginResponse
{ 

    public string Token { get; set; } = string.Empty;
}
    

