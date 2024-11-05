using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class DeleteRegistryPipelineResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid PipelineId { get; set; }
    }
}
