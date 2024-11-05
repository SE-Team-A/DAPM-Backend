using System;
using Microsoft.AspNetCore.Identity;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public interface IUserService
{
    public Task<IdentityResult> CreateUserAsync(string username, string password, bool isAdmin = false);

    public Task<string> LoginUserAsync(string username, string password);
    public Task<List<UserDto>> GetAllUsersAsync(string token);
}
