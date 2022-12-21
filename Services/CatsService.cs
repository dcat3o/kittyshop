using kittyshop.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace kittyshop.Services;

public class CatsService
{
    private readonly IMongoCollection<Cat> _catsCollection;

    public CatsService(IOptions<KittyShopDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _catsCollection = mongoDatabase.GetCollection<Cat>(settings.Value.CatsCollectionName);
    }

    public async Task<List<Cat>> GetAsync() =>
        await _catsCollection.Find(_ => true).ToListAsync();

    public async Task<Cat?> GetAsync(string id) =>
        await _catsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Cat newCat) =>
        await _catsCollection.InsertOneAsync(newCat);

    public async Task UpdateAsync(string id, Cat updatedCat) =>
        await _catsCollection.ReplaceOneAsync(x => x.Id == id, updatedCat);

    public async Task RemoveAsync(string id) =>
        await _catsCollection.DeleteOneAsync(x => x.Id == id);
}