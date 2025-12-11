using Microsoft.Extensions.Configuration;
using Mini.AuthApi.IContract;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mini.AuthApi.Services
{
    public class RabbitMQAuthMessageSender : IRabbitMQAuthMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection connection;
        private IConfiguration configuration;
        public RabbitMQAuthMessageSender(IConfiguration _configuration)
        {
            configuration = _configuration;
            _hostName = configuration.GetValue<string>("RabbitMQEmailSettings:HostName");
            _password = configuration.GetValue<string>("RabbitMQEmailSettings:Password");
            _userName = configuration.GetValue<string>("RabbitMQEmailSettings:UserName");
        }
        public async void SendMessage(object mess, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            //RabbitMq ile baglanti kur
            connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName,
                                     durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var json = JsonConvert.SerializeObject(mess);
            var body = Encoding.UTF8.GetBytes(json);
            //mesaji yayinlama
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

        }
    }
}
