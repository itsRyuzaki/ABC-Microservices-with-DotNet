using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddBrandDTO : ImageDTO
{
    [Required]
    public required string Type { get; set; }
    
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string OfficialSite { get; set; }

}