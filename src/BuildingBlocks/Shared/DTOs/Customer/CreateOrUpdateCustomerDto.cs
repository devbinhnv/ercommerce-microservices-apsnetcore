namespace Shared.DTOs.Customer;

public abstract class CreateOrUpdateCustomerDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }
}

public class  CreateCustomerDto : CreateOrUpdateCustomerDto
{
    public string UserName { get; set; }
}

public class  UpdateCustomerDto : CreateOrUpdateCustomerDto
{
    
}