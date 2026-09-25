namespace TodoTracker.Extensions;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);
        var value = claim?.Value;

        if(Guid.TryParse(value, out Guid guid)) return guid;
       
        throw new UnauthorizedAccessException();
        
    }

}