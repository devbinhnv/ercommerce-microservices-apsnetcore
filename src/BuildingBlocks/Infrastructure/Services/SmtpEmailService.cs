using Contracts.Configurations;
using Contracts.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
using Shared.Services.Email;

namespace Infrastructure.Services;

public class SmtpEmailService : ISmtpEmailService
{
    private readonly ILogger _logger;
    private readonly ISmtpEmailSettings _setting;
    private readonly SmtpClient _smtpClient;

    public SmtpEmailService(ILogger logger, IOptions<SMTPEmailSettings> optionSetting)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _setting = optionSetting.Value ?? throw new ArgumentNullException(nameof(optionSetting));
        _smtpClient = new SmtpClient();
    }

    public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
    {

        var emailMessage = new MimeMessage
        {
            Sender = new MailboxAddress(_setting.DisplayName, request.From ?? _setting.From),
            Subject = request.Subject,
            Body = new BodyBuilder
            {
                HtmlBody = request.Body
            }.ToMessageBody()
        };

        if (request.ToMany.Any())
        {
            foreach (var address in request.ToMany)
            {
                var toAddress = MailboxAddress.Parse(address);
                emailMessage.To.Add(toAddress);
            }
        }
        else
        {
            var toAddress = MailboxAddress.Parse(request.To);
            emailMessage.To.Add(toAddress);
        }

        try
        {
            await _smtpClient.ConnectAsync(
                host: _setting.SMTPServer,
                port: _setting.Port,
                useSsl: _setting.UseSsl,
                cancellationToken);

            await _smtpClient.AuthenticateAsync(
                userName: _setting.Username,
                password: _setting.Password,
                cancellationToken);

            await _smtpClient.SendAsync(emailMessage, cancellationToken);
            await _smtpClient.DisconnectAsync(quit: true, cancellationToken);
        }
        catch (Exception ex)
        {

            _logger.Error(ex.Message, ex);
        }
        finally
        {
            await _smtpClient.DisconnectAsync(quit: true, cancellationToken);
        }
    }
}
