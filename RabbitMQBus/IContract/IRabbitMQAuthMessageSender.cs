namespace RabbitMQBus.IContract
{
    public interface IRabbitMQAuthMessageSender
    {
        void SendMessage(Object mess, string queueName);
    }
}
