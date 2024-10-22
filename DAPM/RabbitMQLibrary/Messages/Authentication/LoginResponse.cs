using RabbitMQLibrary.Interfaces;
using System;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace RabbitMQLibrary.Messages.Authentication
{
    public class LoginResponse : IQueueMessage
    {
        public string Token { get; set; }
        public Guid TicketId { get; set; }
        public string Message { get; set; }
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
    }
}
