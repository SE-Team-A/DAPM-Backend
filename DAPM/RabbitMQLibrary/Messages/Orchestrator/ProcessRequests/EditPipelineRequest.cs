using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace RabbitMQLibrary.Messages.Orchestrator.ProcessRequests
{
    public class EditPipelineRequest : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public Guid OrganizationId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid PipelineId { get; set; }
        
        public String Name { get; set; }

       public Pipeline Pipeline { get; set; }
               
    }
}

