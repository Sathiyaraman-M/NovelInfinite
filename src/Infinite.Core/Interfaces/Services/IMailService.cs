using Infinite.Shared.Requests;

namespace Infinite.Core.Interfaces.Services;

public interface IMailService
{
    Task SendMailAsync(MailRequest mailRequest, string origin);
}