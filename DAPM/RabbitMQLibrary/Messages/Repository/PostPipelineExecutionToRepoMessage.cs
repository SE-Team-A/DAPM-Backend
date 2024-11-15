using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace RabbitMQLibrary.Messages.Repository
{
    public class PostPipelineExecutionToRepoMessage : IQueueMessage
    {
        public Guid TicketId { get; set; }
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid PipelineId { get; set; }
    }
}
