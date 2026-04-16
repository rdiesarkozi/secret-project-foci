using System.Net;
using System.Net.Mail;
using WebAPI.Interfaces;

namespace WebAPI.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient(_configuration["Email:Host"])
        {
            Port = int.Parse(_configuration["Email:Port"]!),
            Credentials = new NetworkCredential(
                _configuration["Email:Username"],
                _configuration["Email:Password"]),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(_configuration["Email:From"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }
}