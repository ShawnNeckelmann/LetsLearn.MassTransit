namespace BurgerLink.Shared.MongDbConfiguration;

public class MongoDbSettings
{
    public string CollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}