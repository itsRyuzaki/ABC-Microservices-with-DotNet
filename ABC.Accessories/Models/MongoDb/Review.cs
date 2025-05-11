using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ABC.Accessories.Models.MongoDb;

public class Review {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    public required string AccessoryGuid { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    public required string UserName { get; set; }

    [Required]
    public int Rating {get; set;}

    [Required]
    public required string ReviewDetails {get; set;}

    [Required]
    public DateTime DatePosted { get; set; }

    public List<BaseImageDetail> Images { get; set; } = [];
}