namespace RabbitMQLibrary.Messages.Authentication
{
    public class PostLoginResponse
    {
        public string Token { get; set; }
        public Guid TicketId { get; set; }
        public string Message { get; set; }
    }
}
