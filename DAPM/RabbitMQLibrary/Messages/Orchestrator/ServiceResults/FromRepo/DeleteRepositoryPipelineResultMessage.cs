using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo
{
    public class DeleteRepositoryPipelineResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
    }
}