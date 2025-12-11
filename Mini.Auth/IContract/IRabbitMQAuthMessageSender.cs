namespace Mini.AuthApi.IContract
{
    public interface IRabbitMQAuthMessageSender
    {
        void SendMessage(Object mess, string queueName);
    }
}
