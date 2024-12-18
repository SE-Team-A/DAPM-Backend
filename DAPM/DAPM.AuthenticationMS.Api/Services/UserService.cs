using System;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using DAPM.ClientApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Services;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRolesService _rolesService;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService, IRolesService rolesService, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _rolesService = rolesService;
        _logger = logger;
    }
    public async Task<IdentityResult> CreateUserAsync(string username, string password, string role)
    {
        var user = new IdentityUser
        {
            UserName = username,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            if (new string[] { "superadmin", "admin", "user", "guest" }.Contains(role.ToLower()))
            {
                await _rolesService.AddRoleToUser(role, username);
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

        if (!result.Succeeded)
        {
            return null;
        }

        return await _tokenService.CreateToken(user);
    }

    public async Task<List<UserDto>> GetAllUsersAsync(string token)
    {
        var users = await _userManager.Users
        .Select(user => new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = null,
            // Map other properties as needed
        })
        .ToListAsync();

        foreach (var userDto in users)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            userDto.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        return users;
    }

    public async Task<bool> DeleteUserFromSystem(string requestToken, Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        // If the request comes from a regular user
        string requestRoleName = _tokenService.GetRoleFromToken(requestToken);
        if (requestRoleName != "Admin" && requestRoleName != "SuperAdmin")
        {
            _logger.LogInformation($"Failed to delete user {userId}, because request role is only {requestRoleName}");
            return false;
        }

        // If the user does not exist
        if (user == null)
        {
            _logger.LogInformation($"Failed to delete user {userId}, because user does not exist.");
            return false;
        }

        // If the original role is a type of admin, it can only be changed by a superadmin
        string? originalRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        if (originalRoleName == "Admin" || originalRoleName == "SuperAdmin")
        {
            if (requestRoleName != "SuperAdmin")
            {
                _logger.LogInformation($"Failed to delete user {userId}, because request role is only {requestRoleName}");
                return false;
            }
        }
        
        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        var deleteUser = await _userManager.DeleteAsync(user);

        return deleteUser.Succeeded;
    }
}