using RabbitMQLibrary.Models;

namespace DAPM.AuthenticationMS.Api.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task <bool> PostLogin(string username, string password);
    }
}
