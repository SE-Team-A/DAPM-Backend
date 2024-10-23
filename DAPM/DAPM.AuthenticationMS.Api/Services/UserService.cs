using System;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRolesService _rolesService;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService, IRolesService rolesService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _rolesService = rolesService;
    }
    public async Task<IdentityResult> CreateUserAsync(string username, string password, bool isAdmin = false)
    {
        var user = new IdentityUser
        {
            UserName = username,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            if (isAdmin)
            {
                await _rolesService.AddRoleToUser("Admin", username);
            }
        }

        return result;  // Return IdentityResult to check if registration was successful
    }

    public async Task<string> LoginUserAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded) {
            return null;
        }

        return await _tokenService.CreateToken(user);
    }
}
