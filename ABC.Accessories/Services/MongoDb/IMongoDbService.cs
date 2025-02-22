using ABC.Accessories.Models.MongoDb;
using MongoDB.Driver;

namespace ABC.Accessories.Services.MongoDb;

public interface IMongoDbService
{
    public Dictionary<string, IMongoCollection<AccessoryExtras>> GetAccExtrasCollectionMap();

    public Dictionary<string, IMongoCollection<AccessoryBaseExtras>> GetBaseExtrasCollectionMap();

}