namespace kittyshop;

public class KittyShopDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string CatsCollectionName { get; set; } = null!;
    public string PositionsCollectionName { get; set; } = null!;
}