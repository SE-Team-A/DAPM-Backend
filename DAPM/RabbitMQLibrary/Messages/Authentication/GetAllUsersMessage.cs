using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace RabbitMQLibrary.Messages.Authentication
{
    public class GetAllUsersMessage : IQueueMessage
    {
        public Guid ProcessId { get; set; }
        public Guid TicketId { get; set; }
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public required string Token { get; set; }
    }
}
