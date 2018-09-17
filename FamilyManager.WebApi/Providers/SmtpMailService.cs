using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace FamilyManager.WebApi.Providers
{
    public class SmtpMailService
    {
        public static void SendtoMail(string tosend, string subject,string body)
        {
            var fromAddress = new MailAddress("tupamba@gmail.com", "From Name");
            var toAddress = new MailAddress(tosend, "To Name");
            const string fromPassword = "Blanquillo0640";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}