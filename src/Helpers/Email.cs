using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Helpers
{
    public static class Email
    {
        private static readonly string _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? throw new ApplicationException("SENDGRID_API_KEY variable not found");
        private static readonly SendGridClient _client = new SendGridClient(_apiKey);

        public static readonly EmailAddress defaultFrom = new EmailAddress("no-reply@sodein.cl", "Contacto AdmInmuebles");
        public static readonly EmailAddress defaultTo = new EmailAddress("test@sodein.cl", "Sodein Test");
        
        public static async Task<Response> Send(SendGridMessage msg)
        {
            return await _client.SendEmailAsync(msg);
        }
        public static async Task<Response> Send(string from, string to, string subject, string plainTextContent, string htmlContent, string attachmentName="", string attachmentBase64String="")
        {
            var msg = MailHelper.CreateSingleEmail(MailHelper.StringToEmailAddress(from), MailHelper.StringToEmailAddress(to), subject, plainTextContent, htmlContent);
            if (attachmentName != string.Empty && attachmentBase64String != string.Empty)
                msg.AddAttachment(attachmentName, attachmentBase64String);
            return await Send(msg);
        }
        public static async Task<Response> SendTemplate(EmailAddress from, List<EmailAddress> to, string subject, string templateId, object templateData, string attachmentName = "", string attachmentBase64String = "")
        {
            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.AddTos(to);
            msg.SetSubject(subject);
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);
            if(attachmentName!=string.Empty && attachmentBase64String != string.Empty)
                msg.AddAttachment(attachmentName, attachmentBase64String);
            return await Send(msg);
        }
        public static async Task<Response> SendTransactional(List<EmailAddress> to, string templateId, object templateData, string subject = "", string attachmentName = "", string attachmentBase64String = "")
        {
            var msg = new SendGridMessage();
            msg.SetFrom(defaultFrom);
            msg.AddTos(to);
            if(subject!="")
                msg.SetSubject(subject);
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);
            if (attachmentName != string.Empty && attachmentBase64String != string.Empty)
                msg.AddAttachment(attachmentName, attachmentBase64String);
            return await Send(msg);
        }
        public static async Task<Response> SendLegacy(EmailAddress from, List<EmailAddress> to, string subject, string templateId, IDictionary<string,string> substitutions, string attachmentName = "", string attachmentBase64String = "")
        {
            var msg = new SendGridMessage();
            msg.SetFrom(from);
            msg.AddTos(to);
            msg.SetSubject(subject);
            msg.SetTemplateId(templateId);
            if (attachmentName != string.Empty && attachmentBase64String != string.Empty)
                msg.AddAttachment(attachmentName, attachmentBase64String);
            substitutions.ToList().ForEach(sub => msg.AddSubstitution(sub.Key, sub.Value));
            return await Send(msg);
        }

        public static class Templates
        {
            public static class Transactional
            {
                public const string PasswordReset = "d-7c06b4d454084580b9d809cd0c93a2e0";
                public const string NewCustomer = "d-9bda9bf90bdd490eb29a204dde5de217";
            } 
        }
    }
}
