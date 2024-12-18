using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;

/// <author>Tamas Drabos</author>

namespace RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRepo
{
    public class GetPipelineExecutionsFromRepoResultMessage: IQueueMessage
    {
        public Guid MessageId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public IEnumerable<PipelineExecution> PipelineExecutions { get; set; }
        public Guid ProcessId { get; set; }
    }
}
