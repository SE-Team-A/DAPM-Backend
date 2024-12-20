using RabbitMQLibrary.Interfaces;

/// <author>Vladyslav Synytskyi</author>

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
