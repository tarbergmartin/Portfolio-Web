using System;
using System.Net.Mail;

namespace Portfolio.API.Classes
{
    public class MailHelper
    {
        private readonly SmtpClient _smtpClient;

        public MailHelper(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public bool SendMail(string subject, string body, string fromAddress, string toAddress)
        {
            try
            {
                using (_smtpClient)
                {
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromAddress),
                    };

                    mailMessage.Subject = subject;
                    mailMessage.To.Add(toAddress);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = body;

                    _smtpClient.Send(mailMessage);
                }

                return true;
            }

            catch (Exception e)
            {
                // Log error
                return false;
            }

        }
    }
}
