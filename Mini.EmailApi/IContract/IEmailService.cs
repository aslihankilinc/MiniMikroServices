namespace Mini.EmailApi.IContract
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}
