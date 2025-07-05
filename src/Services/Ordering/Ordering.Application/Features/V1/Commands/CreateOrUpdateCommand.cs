namespace Ordering.Application.Features.V1.Commands;

public class CreateOrUpdateCommand
{
    public decimal TotalPrice { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    // Address
    public string EmailAddress { get; set; }

    public string ShippingAddress { get; set; }

    public string InvoiceAddress { get; set; }
}
