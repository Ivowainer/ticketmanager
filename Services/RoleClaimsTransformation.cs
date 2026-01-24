using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TicketManager.Models;

namespace TicketManager.Services;

public class RoleClaimsTransformation(UserManager<User> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        // If it already has the role, we don0't need anything
        if (principal.HasClaim(c => c.Type == ClaimTypes.Role))
            return principal;

        // Actual User Id
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null) return principal;

        // User in DB
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return principal;

        var roles = await userManager.GetRolesAsync(user);

        // add the role like "Claims" in memory
        var identity = (ClaimsIdentity)principal.Identity!;
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return principal;
    }
}