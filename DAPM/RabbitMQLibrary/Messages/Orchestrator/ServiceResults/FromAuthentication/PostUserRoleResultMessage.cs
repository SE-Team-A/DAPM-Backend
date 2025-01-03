using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class PostUserRoleResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
        public string? ErrMsg { get; set; }
    }
}