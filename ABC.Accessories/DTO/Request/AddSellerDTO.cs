using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO.Request;

public class AddSellerDTO : BasePayloadDTO
{

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string MobileNumber { get; set; }

    [Required]
    public required string Address { get; set; }

    public string? Website { get; set; }
}