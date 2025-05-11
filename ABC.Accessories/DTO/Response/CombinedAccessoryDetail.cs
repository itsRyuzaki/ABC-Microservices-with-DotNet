using ABC.Accessories.Models;

namespace ABC.Accessories.DTO.Response;

public class CombinedAccessoryDetail
{
    public required string Name { get; set; }

    public required string AccessoryGuid { get; set; }

    public string? Description { get; set; }

    public int AvailableCount { get; set; }

    public List<BaseImageDetail> ImageDetails { get; set; } = [];

    public Decimal AbcPrice { get; set; }

    public string[] Specifications { get; set; } = [];

    public string[] InBoxItems { get; set; } = [];

    public KeyValuePair<string, string[]>[] MasterAttributes { get; set; } = [];

    public IDictionary<string, string> ItemAttributes { get; set; } = new Dictionary<string, string>();

    public List<KeyValuePair<int, string>> Sellers { get; set; } = [];

    public int DeviceModelId { get; set; }

    public int CategoryId { get; set; }

    public int BrandId { get; set; }


}