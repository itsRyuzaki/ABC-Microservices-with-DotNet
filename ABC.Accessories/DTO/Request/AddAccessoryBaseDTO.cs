using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddAccessoryBaseDTO : BasePayloadDTO
{

    [Required]
    public required string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int DeviceModelId { get; set; }

    [Required]
    public int BrandId { get; set; }

    public KeyValuePair<string, string[]>[] MasterAttributes { get; set; } = [];

}