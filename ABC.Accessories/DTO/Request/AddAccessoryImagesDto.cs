using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddAccessoryImagesDTO : BasePayloadDTO
{
    public required string AccessoryGuid { get; set; }

    [Required]
    [MinLength(1)]
    public List<ImageDTO> ItemImages { get; set; } = [];
}
