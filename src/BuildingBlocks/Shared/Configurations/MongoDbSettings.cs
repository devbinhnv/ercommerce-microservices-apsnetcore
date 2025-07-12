namespace Shared.Configurations;
public class MongoDbSettings : DatabaseSettings
{
    public string DBProvider { get; set; }
    public string DatabaseName { get; set; }
}
