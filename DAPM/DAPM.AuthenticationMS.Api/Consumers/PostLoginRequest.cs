using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using RabbitMQLibrary.Messages.Authentication;

namespace DAPM.AuthenticationMS.API.Consumers
{
    public class PostLoginRequestConsumer
    {
        private readonly IModel _channel;
        private readonly string _queueName = "post_login_requests"; // The RabbitMQ queue name

        public PostLoginRequestConsumer(IModel channel)
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
                var loginRequest = JsonSerializer.Deserialize<PostLoginRequest>(message);

                // Process the login request
                ProcessLoginRequest(loginRequest);
                
                // Acknowledge the message
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        }

        private void ProcessLoginRequest(PostLoginRequest request)
        {
            // Implement your login logic here, e.g.:
            // Validate user credentials and generate token

            // Once processed, you can publish a PostLoginResponse message
        }
    }
}
