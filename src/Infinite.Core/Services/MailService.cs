using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Infinite.Core.Interfaces.Services;
using Infinite.Shared.Configurations;
using Infinite.Shared.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infinite.Core.Services;

public class MailService: IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly MailConfiguration _config;
    
    public MailService(IOptions<MailConfiguration> options, ILogger<MailService> logger)
    {
        _logger = logger;
        _config = options.Value;
    }
    
    public async Task SendMailAsync(MailRequest mailRequest, string origin)
    {
        try
        {
            using var smtpClient = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = _config.EnableSsl,
                Host = _config.Host,
                Port = _config.Port,
                Credentials = new NetworkCredential(_config.UserName, _config.Password)
            };
            Email.DefaultSender = new SmtpSender(smtpClient);
            await Email.From(_config.UserName, _config.DisplayName).To(mailRequest.To).Subject(mailRequest.Subject).Body(mailRequest.Body).SendAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error: {Message}", e.Message);
        }
    }
}