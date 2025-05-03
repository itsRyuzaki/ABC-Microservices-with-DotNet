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

    public KeyValuePair<string, string[]>[] MasterAttributes {get; set;} = [];

    public IDictionary<string, string> ItemAttributes { get; set; } = new Dictionary<string, string>();

    public KeyValuePair<int, string>[] SellerNames { get; set; } = [];

    public KeyValuePair<int, string> DeviceModelName { get; set; }

    public KeyValuePair<int, string> CategoryName { get; set; }

    public KeyValuePair<int, string> BrandName { get; set; }


}