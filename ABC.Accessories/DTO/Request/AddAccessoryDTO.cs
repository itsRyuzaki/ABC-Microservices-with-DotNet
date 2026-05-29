using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddAccessoryDTO : BasePayloadDTO
{
    [Required]
    public required string AccessoryBaseId { get; set; }

    [Required]
    [MinLength(1)]
    public int[] SellerIds { get; set; } = [];

    [Required]
    public required string Description { get; set; }

    public string[] Specifications { get; set; } = [];

    [Required]
    public required string[] InBoxItems { get; set; }

    [Required]
    public decimal SellerPrice { get; set; }

    [Required]
    public decimal AbcPrice { get; set; }
    [Required]
    public decimal OriginalPrice { get; set; }

    [Required]
    public int AvailableCount { get; set; }

    public IDictionary<string, string> ItemAttributes { get; set; } = new Dictionary<string, string>();

}