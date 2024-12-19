using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

/// <author>Tamás Drabos</author>

namespace RabbitMQLibrary.Messages.PipelineOrchestrator
{
    public class GetPipelineExecutionFromRepoResultMessage: IQueueMessage
    {
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public PipelineExecution Execution { get; set; }
        public PipelineDTO Pipeline { get; set; }
    }
}
