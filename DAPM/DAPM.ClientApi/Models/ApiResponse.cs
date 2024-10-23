using System.Text.Json.Serialization;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace DAPM.ClientApi.Models
{
    public class ApiResponse
    {
        public string RequestName { get; set; }
        public Guid TicketId { get; set; }
        public string Message { get; internal set; }
    }
}
