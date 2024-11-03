using System;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public interface IUserService
{
    public Task<IdentityResult> CreateUserAsync(string username, string password, bool isAdmin = false);

    public Task<string> LoginUserAsync(string username, string password); 
}
