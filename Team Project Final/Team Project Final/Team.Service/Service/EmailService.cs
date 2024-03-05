using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Team.Service.Interface;

namespace Team.Service.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string to, string subject, string body)
        {
            using (SmtpClient smtpClient = new SmtpClient("smtp.Gmail.com"))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("nilayawsin0812@gmail.com", "gdfv rbow hbta qfax");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage("nilayawsin0812@gmail.com", to, subject, body);
                mailMessage.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
