using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using RabbitMQLibrary.Messages.Authentication;

namespace DAPM.AuthenticationMS.API.Consumers
{
    public class PostLoginResponseConsumer
    {
        private readonly IModel _channel;
        private readonly string _queueName = "post_login_responses"; // The RabbitMQ queue name

        public PostLoginResponseConsumer(IModel channel)
        {
            _channel = channel;
        }

        public void StartConsuming()
        {
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var loginResponse = JsonSerializer.Deserialize<PostLoginResponse>(message);

                // Process the login response
                ProcessLoginResponse(loginResponse);
                
                // Acknowledge the message
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }

        private void ProcessLoginResponse(PostLoginResponse response)
        {
            // Handle the login response, e.g., logging or notifying users
        }
    }
}
