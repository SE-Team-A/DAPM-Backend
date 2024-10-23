using RabbitMQLibrary.Interfaces;

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