using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.ClientApi
{
    public class DeleteRegistryPipelineResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid PipelineId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
    }
}
