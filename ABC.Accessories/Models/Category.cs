using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Category : BaseImageDetail
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }
    public required string Guid { get; set; }

    public List<AccessoryBase> AccessoriesBase { get; set; } = [];

}