using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

/// <author>Vladyslav Synytskyi</author>

namespace DAPM.ClientApi.Consumers
{
    public class DeleteUserResultConsumer : IQueueConsumer<DeleteUserProcessResult>
    {
        private ILogger<DeleteUserResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public DeleteUserResultConsumer(ILogger<DeleteUserResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(DeleteUserProcessResult message)
        {
            _logger.LogInformation("DeleteUserResult received");


            // Objects used for serialization
            JToken result = new JObject();

            result["succeeded"] = message.Succeeded;
            result["errMsg"] = message.ErrMsg;

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}