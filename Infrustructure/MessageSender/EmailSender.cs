using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.MessageSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSetting;

        public EmailSender(IOptions<EmailSettings> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(_emailSetting.Sender, _emailSetting.Password);

                // Mail message
                var mail = new MailMessage
                {
                    From = new MailAddress(_emailSetting.Sender, _emailSetting.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient
                {
                    Port = _emailSetting.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSetting.MailServer,
                    EnableSsl = _emailSetting.Sender.Contains("gmail"),
                    Credentials = credentials
                };

                // Send it...         
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception

                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
