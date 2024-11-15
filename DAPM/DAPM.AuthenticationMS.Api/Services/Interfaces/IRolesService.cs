

/// <author>Ákos Gelencsér</author>
namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IRolesService

    {
        public Task CreateRoleAsync(string roleName);

        public Task AddRoleToUser(string roleName, string userName);

        public Task<string?> SetUserRole(string RequestToken, Guid userId, string roleName);

        public Task DeleteUserRole(string RequestToken, Guid userId, string roleName);
    }

}