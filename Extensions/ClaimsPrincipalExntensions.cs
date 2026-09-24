namespace TodoTracker.Extensions;

using System.Security.Claims;
using TodoTracker.Models;

public static class ClaimsPrincipalExntensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);
        var value = claim?.Value;

        if(Guid.TryParse(value, out Guid guid)) return guid;
       
        throw new UnauthorizedAccessException();
        
    }

}