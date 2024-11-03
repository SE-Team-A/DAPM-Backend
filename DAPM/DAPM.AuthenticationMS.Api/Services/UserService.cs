using System;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task<string> GetAllUsersAsync(string token)
    {
        var users = await _userManager.Users
        .Select(user => new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
            // Map other properties as needed
        })
        .ToListAsync();

        return await _tokenService.CreateToken(users);
    }
}
public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    // Add other properties you want to expose
}
