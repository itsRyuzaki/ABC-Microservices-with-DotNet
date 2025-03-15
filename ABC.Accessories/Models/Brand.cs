using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Brand : BaseImageDetail
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string OfficialSite { get; set; }

    public required string Guid { get; set; }

    public List<AccessoryBase> AccessoriesBase { get; set; } = [];

}