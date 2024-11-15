using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class DeleteUserRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public string RequestToken { get; set; }
        public Guid UserId { get; set; }
    }
}