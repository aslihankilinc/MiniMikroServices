using Mini.EmailApi.IContract;

namespace Mini.EmailApi.Services
{
    public class EmailService : IEmailService
    {
        public async Task  SendEmail(string to, string subject, string body)
        {
           Console.WriteLine($"Sending email to: {to}, Subject: {subject}, Body: {body}");
        }
    }
}
