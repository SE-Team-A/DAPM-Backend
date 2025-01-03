using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
     // AYAT AL RIFAI
namespace RabbitMQLibrary.Messages.ClientApi
{
    public class DeleteResourceFromRepoResult : IQueueMessage
    {
        public Guid MessageId { get; set; }
        public Guid TicketId { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public Guid resourceId { get; set; }
        public Guid organizationId { get; set; }
        public Guid repositoryId { get; set; }
         
    }
}
