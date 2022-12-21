using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace kittyshop.Models;

public class Cat
{
    [BsonId]
    [BsonRepresentation((BsonType.ObjectId))]
    public string? Id { get; set; }
    [Required] public string Breed { get; set; }
    [Required] public string Sex { get; set; }
    [Required] public string Color { get; set; }
    [Required] public int AgeMonths { get; set; }

    public Cat(string breed, string sex, string color, int ageMonths)
    {
        Breed = breed.ToLower();
        Sex = sex.ToLower();
        Color = color.ToLower();
        AgeMonths = ageMonths;  
    }
}