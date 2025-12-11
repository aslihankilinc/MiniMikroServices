using Mini.EmailApi.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mini.EmailApi.RabbitMQ
{
    public class RabbitMQAuthConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IChannel _channel;
        public static string queueName;

        public RabbitMQAuthConsumerService(IConfiguration configuration, EmailService emailService)
        {
            queueName = configuration.GetValue<string>("RabbitMQEmailSettings:AuthQueueName");
            _configuration = configuration;
            _emailService = emailService;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("RabbitMQEmailSettings:HostName"),
                UserName = _configuration.GetValue<string>("RabbitMQEmailSettings:UserName"),
                Password = _configuration.GetValue<string>("RabbitMQEmailSettings:Password")
            };
            _connection = factory.CreateConnectionAsync().Result;
            _channel = _connection.CreateChannelAsync().Result;
            _channel.QueueDeclareAsync(queueName
                , true, false, false, null);
        }
        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (ch, ea) =>
            {
                try
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    string email = content;

                    Console.WriteLine($"Rabbit Mail :{email}");
                    await HandleMessage(email);

                    await _channel.BasicAckAsync(ea.DeliveryTag, false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rabbit Mail Hata:{ex.ToString()}"
                 );
                }

                _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(queueName, false, consumer);

            return Task.CompletedTask;
        }
        private async Task HandleMessage(string email)
        {
            await _emailService.SendEmail("test", "kuyruğa girdi", email);
        }
    }
}
