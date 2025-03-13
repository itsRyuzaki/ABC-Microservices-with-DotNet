using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Brand {
    public int Id {get; set;}

    [Required]
    public required string Name {get; set;}

    [Required]
    public required string OfficialSite {get; set;}

    public required string Guid { get; set; }

    public List<AccessoryBase> AccessoriesBase { get; set; } = [];

}