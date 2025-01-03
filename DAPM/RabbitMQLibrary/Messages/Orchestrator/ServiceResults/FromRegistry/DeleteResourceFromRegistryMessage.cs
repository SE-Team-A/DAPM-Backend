using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <author>Ayat Al Rifai</author>
namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry
{
    public class DeleteResourceFromRegistryResultMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public Guid ResourceId { get; set; }
         public Guid organizationId { get; set; }
         public  Guid repositoryId { get; set; }
    }
}
