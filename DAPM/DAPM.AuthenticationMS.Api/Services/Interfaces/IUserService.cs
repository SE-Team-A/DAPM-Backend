using System;
using Microsoft.AspNetCore.Identity;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;

public interface IUserService
{
    public Task<IdentityResult> CreateUserAsync(string username, string password);

    public Task<string> LoginUserAsync(string username, string password);
}
