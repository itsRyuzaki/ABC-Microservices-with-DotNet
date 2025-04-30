using ABC.Accessories.Models;

namespace ABC.Accessories.DTO.Response;


public class CombinedAccessoryDetail
{
    public required string Name { get; set; }

    public required string AccessoryGuid { get; set; }

    public string? Description { get; set; }

    public List<BaseImageDetail> ImageDetails { get; set; } = [];

    public Decimal AbcPrice { get; set; }

}