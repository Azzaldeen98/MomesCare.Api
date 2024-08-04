using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MomesCare.Api.Helpers;

public interface IUserClaimsHelper
{
    string UserId { get; }
    string UserRole { get; }    
    string Email { get; }

}

public class UserClaimsHelper : IUserClaimsHelper
{

    public UserClaimsHelper(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor?.HttpContext?.User?.GetClaim(JwtClaimTypes.Id) ?? "";
        UserRole = httpContextAccessor?.HttpContext?.User?.GetClaim(ClaimTypes.Role);
        Email = httpContextAccessor?.HttpContext?.User?.GetClaim(JwtClaimTypes.Email);
 
    }

    public string UserId { get; private set; }

    public string UserRole { get; private set; }

    public string Email { get; private set; }


}
