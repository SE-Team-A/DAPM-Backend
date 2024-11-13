using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Authentication
{
    public class PostUserRoleMessage : IQueueMessage
    {
        public Guid ProcessId { get; set; }
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public string RequestToken { get; set; }
        public Guid UserId { get; set; }
        public string RoleName { get; set; }
    }
}