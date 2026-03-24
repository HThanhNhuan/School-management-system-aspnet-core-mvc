using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SchoolManagementSystem.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response SendEmail(string to, string subject, string body)
        {
            var nameFrom = _configuration["Mail:NameFrom"];
            var from = _configuration["Mail:From"];
            var smtp = _configuration["Mail:Smtp"];
            var port = _configuration["Mail:Port"];
            var password = _configuration["Mail:Password"];

            if (string.IsNullOrWhiteSpace(from))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Mail:From is missing in appsettings.json"
                };
            }

            if (string.IsNullOrWhiteSpace(smtp))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Mail:Smtp is missing in appsettings.json"
                };
            }

            if (string.IsNullOrWhiteSpace(port))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Mail:Port is missing in appsettings.json"
                };
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Mail:Password is missing in appsettings.json"
                };
            }

            if (string.IsNullOrWhiteSpace(to))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Recipient email is empty"
                };
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom ?? "System", from));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodybuilder.ToMessageBody();

            try
            {
                using var client = new SmtpClient();
                client.Connect(smtp, int.Parse(port), SecureSocketOptions.StartTls);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return new Response
            {
                IsSuccess = true
            };
        }
    }
}