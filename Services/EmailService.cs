using Microsoft.Extensions.Options;
using Services.Abstractions;
using Shared;
using Shared.Options;
using System.Net;
using System.Net.Mail;

namespace Services
{
    public class EmailService(IOptions<SmtpOptions> options) : IEmailService
    {
        public async Task SendEmailAsync(Email email)
        {

            var SmtpSettings = options.Value;


            using var Client = new SmtpClient(SmtpSettings.Host, SmtpSettings.Port);


            Client.UseDefaultCredentials = false; // Add this
            Client.EnableSsl = SmtpSettings.EnableSsl;




            Client.Credentials = new NetworkCredential(SmtpSettings.Username, SmtpSettings.Password);




            MailMessage mailMessage = new MailMessage()
            {

                From = new MailAddress(SmtpSettings.Username),
                Subject = email.Subject,
                Body = email.Body

            };


            mailMessage.To.Add(email.To);


            await Client.SendMailAsync(mailMessage); // Takes MailMessage 

        }
    }
}
