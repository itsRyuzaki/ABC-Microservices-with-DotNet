using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddCategoryDTO : ImageDTO
{
    [Required]
    public required string Type { get; set; }
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

}