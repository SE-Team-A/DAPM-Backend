using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

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
