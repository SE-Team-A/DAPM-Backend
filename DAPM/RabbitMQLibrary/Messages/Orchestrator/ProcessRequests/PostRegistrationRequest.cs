using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class PostRegistrationRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
