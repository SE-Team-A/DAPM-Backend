using RabbitMQLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <author>Ayat Al Rifai</author>

/// <author>Tamas Drabos</author>

namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class DeleteResourceRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid ResourceId { get; set; }
    }
}

