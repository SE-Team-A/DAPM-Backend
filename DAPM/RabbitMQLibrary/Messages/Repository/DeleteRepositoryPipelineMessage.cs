using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Repository
{
    public class DeleteRepositoryPipelineMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid PipelineId { get; set; }
    }
}
