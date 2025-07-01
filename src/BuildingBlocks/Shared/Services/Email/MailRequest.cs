using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Services.Email;

public class MailRequest
{
    [EmailAddress]
    public string From { get; set; }

    [EmailAddress]
    public string To { get; set; }

    public List<string> ToMany { get; set; } = [];

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Body { get; set; }

    public IFormFileCollection Attachment { get; set; }
}
