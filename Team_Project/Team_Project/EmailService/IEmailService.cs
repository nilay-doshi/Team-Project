namespace Team_Project.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}
