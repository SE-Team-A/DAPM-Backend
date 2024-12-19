using RabbitMQLibrary.Models;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface IHttpService
    {
        public Task<string> SendPostRequestAsync(string url, string body);
        public Task<bool> verifyExternalToken(string externalDomain, string token);
    }
}
