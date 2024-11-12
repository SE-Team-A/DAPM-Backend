using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <author>Nicolai Veiglin Arends</author>
/// <author>Tamás Drabos</author>
namespace RabbitMQLibrary.Messages.PipelineOrchestrator
{
    public class PipelineExecutionMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }

        public PipelineExecution PipelineExecution { get; set; }
    }
}
