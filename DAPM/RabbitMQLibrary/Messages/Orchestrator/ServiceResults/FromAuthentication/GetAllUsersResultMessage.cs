using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>
/// <author>Nicolai Veiglin Arends</author>
/// <author>Thøger Bang Petersen</author>
namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class GetAllUsersResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public List<UserDto> UserDtos { get; set; }

    }

    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
