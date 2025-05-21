using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;  // <--- Interfaz

namespace GestionInventario_MVC.Services
{
    public class EmailSender : IEmailSender  // <--- Implementa la interfaz
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Este es el m�todo que la interfaz requiere
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromAddress = _configuration["EmailSettings:From"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);

            if (string.IsNullOrEmpty(fromAddress))
                throw new InvalidOperationException("La dirección 'From' no está configurada en appsettings.json.");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(fromAddress));
            emailMessage.To.Add(MailboxAddress.Parse(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username, password);
            await smtp.SendAsync(emailMessage);
            await smtp.DisconnectAsync(true);
        }


    }
}
