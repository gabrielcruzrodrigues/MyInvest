namespace MyInvestAPI.Services.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message);
        public Task Execute(string subject, string message, string toEmail);
    }
}
