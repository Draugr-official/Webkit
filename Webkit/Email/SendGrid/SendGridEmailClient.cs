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
    public class SendGridEmailClient
    {
        SendGridClient SendGridClient { get; set; }

        EmailAddress Sender { get; set; }

        public SendGridEmailClient(string apiKey, EmailAddress sender)
        {
            Sender = sender;

            SendGridClient = new SendGridClient(apiKey);
        }

        /// <summary>
        /// ACCEPTED = Good
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        /// <param name="rawBody"></param>
        /// <returns></returns>
        public async Task<Response> Send(EmailAddress recipient, string subject, string htmlBody, string rawBody = "")
            => await SendGridClient.SendEmailAsync(MailHelper.CreateSingleEmail(Sender, recipient, subject, rawBody == "" ? htmlBody : rawBody, htmlBody));
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
