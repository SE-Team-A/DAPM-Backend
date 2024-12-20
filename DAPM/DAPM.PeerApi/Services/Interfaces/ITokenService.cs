using RabbitMQLibrary.Models;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface ITokenService
    {
        public string createToken();
        public bool checkSignature(string token);
    }
}