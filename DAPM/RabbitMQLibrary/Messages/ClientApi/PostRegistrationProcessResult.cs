using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace RabbitMQLibrary.Messages.ClientApi
{
    public class PostRegistrationProcessResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }

    }
}