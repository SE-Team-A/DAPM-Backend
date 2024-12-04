using RabbitMQLibrary.Interfaces;

/// <author>Raihanullah Mehran</author>
namespace RabbitMQLibrary.Messages.ClientApi
{
    public class DeleteRepositoryPipelineResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid PipelineId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
    }
}
