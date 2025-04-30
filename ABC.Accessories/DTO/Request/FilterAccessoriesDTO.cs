using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class FilterAccessoriesDTO
{

    [Required]
    public required string Type { get; set; }

    public string[] SearchTerm { get; set; } = [];

    public int[] CategoryId { get; set; } = [];
    public int[] DeviceModelId { get; set; } = [];
    public int[] BrandId { get; set; } = [];

}