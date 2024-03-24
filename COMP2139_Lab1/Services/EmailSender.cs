
using COMP2139_Lab1.Areas.ProjectManagement.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNet.Identity;




namespace COMP2139_Lab1.Services.Microsoft.Extensions.Configuration
{
    public class EmailSender : IEmailSender
    {

        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return ConfigSendGridAsync(email, subject, htmlMessage);
        }

        private Task ConfigSendGridAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("teimur.terchyyev@georgebrown.ca", "Teimur");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(msg);
        }
    }

}




/*

*/