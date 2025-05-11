using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class FilterAccessoriesDTO
{

    [Required]
    public required string Type { get; set; }

    public string[] SearchTerm { get; set; } = [];

    public int[] CategoryIds { get; set; } = [];
    public int[] DeviceModelIds { get; set; } = [];
    public int[] BrandIds { get; set; } = [];

}