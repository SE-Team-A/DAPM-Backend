using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <author>Ayat Al Rifai</author>
/// <author>Thøger Bang Petersen</author>
namespace RabbitMQLibrary.Messages.ResourceRegistry
{
    public class EditPipelineToRegistryMessage : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid ProcessId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public PipelineDTO Pipeline { get; set; }
        public Guid PipelineId { get; set; }

    }
}
