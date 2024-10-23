using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class PostRegistrationResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
    }
}
