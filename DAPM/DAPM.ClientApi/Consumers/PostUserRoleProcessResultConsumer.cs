using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;

/// <author>Ákos Gelencsér</author>
/// <author>Vladyslav Synytskyi</author>

namespace DAPM.ClientApi.Consumers
{
    public class PostUserRoleProcessResultConsumer : IQueueConsumer<PostUserRoleProcessResult>
    {
        private ILogger<PostUserRoleProcessResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public PostUserRoleProcessResultConsumer(ILogger<PostUserRoleProcessResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(PostUserRoleProcessResult message)
        {
            _logger.LogInformation("PostUserRoleProcessResult received");


            // Objects used for serialization
            JToken result = new JObject();
            // JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            // JToken loginResult = JToken.FromObject(result, serializer);

            //Serialization
            //result["itemIds"] = idsJSON;
            //result["itemType"] = message.ItemType;
            result["succeeded"] = message.Succeeded;
            result["errMsg"] = message.ErrMsg;
            //result["message"] = message.Message;  

            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);

            return Task.CompletedTask;
        }
    }
}