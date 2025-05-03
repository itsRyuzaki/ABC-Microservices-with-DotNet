using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Accessories.Models.MongoDb;

public class AccessoryBaseExtras {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string AccessoryBaseId {get; set;}

    public KeyValuePair<string, string[]>[] MasterAttributes {get; set;} = [];
}