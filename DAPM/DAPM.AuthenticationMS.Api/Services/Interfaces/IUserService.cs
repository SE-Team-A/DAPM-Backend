using System;
using Microsoft.AspNetCore.Identity;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
public interface IUserService
{
    public Task<IdentityResult> CreateUserAsync(string username, string password, string role);

    public Task<string> LoginUserAsync(string username, string password);
    public Task<List<UserDto>> GetAllUsersAsync(string token);
    public Task DeleteUserFromSystem(string requestToken, Guid userId);
}
