using RabbitMQLibrary.Interfaces;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace RabbitMQLibrary.Messages.Authentication
{
    public class PostRegistrationMessage : IQueueMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } 

        public Guid ProcessId { get; set; }
        public Guid TicketId { get; set; }
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
    }
}