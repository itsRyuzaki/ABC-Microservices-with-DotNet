using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Accessories.Models.MongoDb;

public class AccessoryExtras
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string AccessoryBaseId {get; set;}

    public required string AccessoryGuid {get; set;}

    public string[] Specifications { get; set; } = [];

    public required string[] InBoxItems { get; set; }

    public IDictionary<string, string> ItemAttributes {get; set;} = new Dictionary<string, string>();
}