


namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IRolesService

    {
        public Task CreateRoleAsync(string roleName);

        public Task AddRoleToUser(string roleName, string userName);
    }

}