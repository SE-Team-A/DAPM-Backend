using RabbitMQLibrary.Models;

namespace DAPM.PeerApi.Services.Interfaces
{
    public interface ITokenService
    {
        public string createToken();
        public bool checkSignature(string token);
    }
}