using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace PresentationLayer.Areas.Identity.Claims
{
    public class RoleClaimsTransformation : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> userManager;
        public RoleClaimsTransformation(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity;
            if (!identity.IsAuthenticated)
            {
                return principal;
            }
            var user = await userManager.GetUserAsync(principal);
            if(user == null)
            {
                return principal;
            }
            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {

                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            return principal;
        }
    }
}
