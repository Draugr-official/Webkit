using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Webkit.Email.SendGrid
{
    public class SendGridEmail
    {
        SendGridClient SendGridClient { get; set; }

        EmailAddress Sender { get; set; }

        EmailAddress Recipient { get; set; }

        public SendGridEmail(EmailAddress sender, EmailAddress recipient, string apiKey)
        {
            Sender = sender;
            Recipient = recipient;

            SendGridClient = new SendGridClient(apiKey);
        }

        public async Task<Response> Send(string subject, string body, string fallbackBody = "")
            => await SendGridClient.SendEmailAsync(MailHelper.CreateSingleEmail(Sender, Recipient, subject, fallbackBody == "" ? body : fallbackBody, body));
    }

    public static class EmailAddressExtensions
    {
        /// <summary>
        /// Determines if the email address is valid
        /// </summary>
        /// <param name="emailAddress"></param>
        public static bool IsValid(this EmailAddress emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress.Email))
                return false;

            try
            {
                return Regex.IsMatch(emailAddress.Email, @"(.*)@([a-zA-Z0-9\\-].*)\\.([a-zA-Z].*)", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500));
            }
            catch
            {
                return false;
            }
        }
    }
}
