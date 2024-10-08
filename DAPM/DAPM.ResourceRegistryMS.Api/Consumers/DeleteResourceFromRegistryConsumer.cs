using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ResourceRegistry;

namespace DAPM.ResourceRegistryMS.Api.Consumers
{
    public class DeleteResourceFromRegistryConsumer: IQueueConsumer<DeleteResourceFromRegistryMessage>
    {
    }
}
