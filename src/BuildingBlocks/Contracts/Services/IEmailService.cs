namespace Contracts.Services;

public interface IEmailService<in T> where T : class
{
    public Task SendEmailAsync(T request, CancellationToken cancellationToken = default);
}
