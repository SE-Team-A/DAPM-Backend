using DAPM.ClientApi.Services.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQLibrary.Interfaces;
using RabbitMQLibrary.Messages.ClientApi;
using RabbitMQLibrary.Models;
using RabbitMQLibrary.Messages.Orchestrator.ServiceResults.FromRegistry;

namespace DAPM.ClientApi.Consumers
{
    public class GetAllUsersProcessResultConsumer : IQueueConsumer<GetAllUsersProcessResult>
    {
        private ILogger<PostItemResultConsumer> _logger;
        private readonly ITicketService _ticketService;
        public GetAllUsersProcessResultConsumer(ILogger<PostItemResultConsumer> logger, ITicketService ticketService)
        {
            _logger = logger;
            _ticketService = ticketService;
        }

        public Task ConsumeAsync(GetAllUsersProcessResult message)
        {
            _logger.LogInformation("GetAllUsersResultMessage received");


            IEnumerable<UserDto> usersDTOs = message.Users;

            // Objects used for serialization
            JToken result = new JObject();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });


            //Serialization
            JToken usersJSON = JToken.FromObject(usersDTOs, serializer);
            result["users"] = usersJSON;


            // Update resolution
            _ticketService.UpdateTicketResolution(message.TicketId, result);
            
            return Task.CompletedTask;
        }
    }
}