using System.Text.Json.Serialization;

namespace DAPM.ClientApi.Models
{
    public class ApiResponse
    {
        public string RequestName { get; set; }
        public Guid TicketId { get; set; }
        public string Message { get; internal set; }
    }
}
