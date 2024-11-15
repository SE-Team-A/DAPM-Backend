using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
namespace RabbitMQLibrary.Messages.ClientApi
{
    public class DeleteUserProcessResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
        public string? ErrMsg { get; set; }
    }
}