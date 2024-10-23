/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ClientApi.Models.DTOs
{
    public class LoginRequestDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}