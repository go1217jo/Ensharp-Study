using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace MemberManager
{
    class Mail
    {
        private MailAddress sendAddress;
        private MailAddress toAddress;
        private string sendPassword = "1q2w3e4r!@";

        SmtpClient smtp;
        MailMessage message;

        public Mail(string sendMail)
        {
            this.sendAddress = new MailAddress(sendMail);
        }

        public void SetToAddress(string toMail)
        {
            this.toAddress = new MailAddress(toMail);
        }

        public string SendEmail(string subject, string body)
        {
            smtp = new SmtpClient {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(sendAddress.Address, sendPassword),
                Timeout = 20000
            };

            message = new MailMessage(sendAddress, toAddress) {
                Subject = subject,
                Body = body
            };
            smtp.Send(message);

            return "send mail ok";
        }

        public void Close()
        {
            if (smtp != null)
                smtp.Dispose();
            if (message != null)
                message.Dispose();
        }
    }
}
