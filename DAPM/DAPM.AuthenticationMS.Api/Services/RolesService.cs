using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAPM.ClientApi.Services
/// <author>Ákos Gelencsér</author>
{
    public class RolesService : IRolesService
    {
        private readonly ILogger<RolesService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public RolesService(ILogger<RolesService> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public async Task AddRoleToUser(string roleName, string userName)
        {
            //var role = await _roleManager.FindByNameAsync(roleName);
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return;
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return;
        }

        public async Task<string?> SetUserRole(string requestToken, Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            
            if (user == null) {
                _logger.LogInformation($"Failed to set role {roleName} for UserID {userId}, because user does not exist.");
                return "User does not exist";
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) {
                _logger.LogInformation($"Failed to set role {roleName} for UserID {userId}, because role does not exist.");
                return "Role does not exist";
            }

            // If the request comes from a regular user
            string requestRoleName = _tokenService.GetRoleFromToken(requestToken);
            if (requestRoleName != "Admin" && requestRoleName != "SuperAdmin") {
                _logger.LogInformation($"Failed to set role {roleName} for UserID {userId}, because request role is only {requestRoleName}");
                return "You don't have the privileges to preform this operation.";
            }

            // If the original role is a type of admin, it can only be changed by a superadmin
            string? originalRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (originalRoleName == "Admin" || originalRoleName == "SuperAdmin") {
                if (requestRoleName != "SuperAdmin") {
                    _logger.LogInformation($"Failed to set role {roleName} for UserID {userId}, because request role is only {requestRoleName}");
                    return "You don't have the privileges to preform this operation.";
                }
            }

            // If the new role is superadmin, it can only be set by a superadmin
            if (roleName == "SuperAdmin" && requestRoleName != "SuperAdmin") {
                _logger.LogInformation($"Failed to set role {roleName} for UserID {userId}, because request role is only {requestRoleName}");
                return "You don't have the privileges to preform this operation.";
            }

            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRoleAsync(user, roleName);

            return null;
        }
    }
}