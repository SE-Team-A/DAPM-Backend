using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAPM.ClientApi.Services

{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
    }
}