using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class BasePayloadDTO
{

    [Required]
    public required string Type { get; set; }
}