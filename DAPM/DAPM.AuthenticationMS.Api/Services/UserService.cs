using System;
using DAPM.AuthenticationMS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    public async Task<IdentityResult> CreateUserAsync(string username, string password)
    {
        var user = new IdentityUser
        {
            UserName = username,
        };

        var result = await _userManager.CreateAsync(user, password);
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

        return _tokenService.CreateToken(user);
    }
}
