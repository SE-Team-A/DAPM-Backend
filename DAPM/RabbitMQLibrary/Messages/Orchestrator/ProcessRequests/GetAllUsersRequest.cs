using RabbitMQLibrary.Interfaces;

/// <author>Vladyslav Synytskyi</author>

namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class GetAllUsersRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public required string Token { get; set; }
    }
}
