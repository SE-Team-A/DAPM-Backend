using DAPM.ClientApi.Models;
using System.Xml.Linq;

namespace DAPM.ClientApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Guid PostAuthentication (string username, string password);

    }
}
