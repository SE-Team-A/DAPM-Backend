using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLibrary.Interfaces;

namespace RabbitMQLibrary.Messages.Repository
{
    public class GetPipelineExecutionsFromRepoMessage: IQueueMessage
    {
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid PipelineId { get; set; }
        public Guid ProcessId { get; set; }
        public Guid? ExecutionId { get; set; }
    }
}
