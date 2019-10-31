using CoreApp.Data.Entities;
using CoreApp.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreApp.Helpers
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private readonly UserManager<AppUser> _userManager;

        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManager = userManager;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim(CommonConstants.UserId, user.Id.ToString() ?? string.Empty),
                new Claim(CommonConstants.Email,user.Email?? string.Empty),
                new Claim(CommonConstants.FullName,user.FullName?? string.Empty),
                new Claim(CommonConstants.Avatar,user.Avatar?? string.Empty),
                new Claim(CommonConstants.UserClaims.Roles,string.Join(CommonConstants.SepRoles,roles))
            });
            return principal;
        }
    }
}