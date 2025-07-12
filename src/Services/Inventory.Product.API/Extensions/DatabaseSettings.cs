namespace Inventory.Product.API.Extensions;

public class DatabaseSettings : Shared.Configurations.DatabaseSettings
{
    public string DBProvider { get; set; }
    public string DatabaseName { get; set; }
}
