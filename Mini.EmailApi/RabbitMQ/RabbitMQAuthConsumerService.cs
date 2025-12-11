namespace Mini.EmailApi.RabbitMQ
{
    public class RabbitMQAuthConsumerService:BackgroundService
    {
        internal class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
        {
            private readonly string _hostName;
            private readonly string _password;
            private readonly string _userName;
            private readonly string _queueName;
            private readonly IConfiguration _configuration;
            private IConnection _connection;
            private IChannel _channel;
            public RabbitMQAuthMessageSender(IConfiguration configuration)
            {
                _configuration = configuration;
                _queueName = _configuration.GetValue<string>("RabbitMQEmailSettings:AuthQueueName");
                _hostName = _configuration.GetValue<string>("RabbitMQEmailSettings:HostName");
                _password = _configuration.GetValue<string>("RabbitMQEmailSettings:Password");
                _userName = _configuration.GetValue<string>("RabbitMQEmailSettings:UserName");

                _connection = factory.CreateConnectionAsync().Result;
                _channel = _connection.CreateChannelAsync().Result;
                _channel.QueueDeclareAsync(_queueName
                    , true, false, false, null);
            }

            public void SendMessage(object mess, string queueName)
            {
                // Implementation for sending message
            }
        }
}
