using DAPM.ClientApi.Models;
using RabbitMQLibrary.Messages.Authentication;
using System.Xml.Linq;
/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IAuthenticationService

    {
        public Guid PostLogin(string username, string password);
        public Guid PostRegistration(string username, string password, string name, string role);
    }

}
