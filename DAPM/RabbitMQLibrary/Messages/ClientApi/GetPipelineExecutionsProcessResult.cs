using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

namespace RabbitMQLibrary.Messages.ClientApi
{
    public class GetPipelineExecutionsProcessResult: IQueueMessage
    {
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid TicketId { get; set; }
        public IEnumerable<PipelineExecution> PipelineExecutions { get; set; }
    }
}
