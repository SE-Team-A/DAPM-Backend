namespace RabbitMQLibrary.Messages.Authentication
{
    public class PostLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid TicketId { get; set; }
    }
}
