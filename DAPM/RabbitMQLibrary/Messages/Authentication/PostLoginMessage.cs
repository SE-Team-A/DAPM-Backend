using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Authentication
{
    public class PostLoginMessage : IQueueMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid TicketId { get; set; }
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
    }
}
