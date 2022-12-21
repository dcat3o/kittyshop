using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace kittyshop.Models;

public class Position
{
    [BsonId]
    [BsonRepresentation((BsonType.ObjectId))]
    public string? Id { get; set; }

    [Required] public MongoDBRef Cat { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateEdited { get; set; }
    [Required] public decimal Price { get; set; }

    public Position(MongoDBRef cat, decimal price)
    {
        Cat = cat;
        DateCreated = DateTime.Now;
        Price = price;
    }

    public Position(Cat cat, decimal price)
    {
        Cat = new MongoDBRef("cats", cat.Id);
        Price = price;
    }
}