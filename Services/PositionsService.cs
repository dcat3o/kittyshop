using kittyshop.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace kittyshop.Services;

public class PositionsService
{
    private readonly IMongoCollection<Position> _positionsCollection;

    public PositionsService(IOptions<KittyShopDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _positionsCollection = mongoDatabase.GetCollection<Position>(settings.Value.PositionsCollectionName);
    }

    public async Task<List<Position>> GetAsync()
    {
        return await _positionsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Position?> GetAsync(string id) =>
        await _positionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Position newPosition)
        => await _positionsCollection.InsertOneAsync(newPosition);

    public async Task UpdateAsync(string id, Position updatedPosition) =>
        await _positionsCollection.ReplaceOneAsync(x => x.Id == id, updatedPosition);

    public async Task RemoveAsync(string id) =>
        await _positionsCollection.DeleteOneAsync(x => x.Id == id);
}