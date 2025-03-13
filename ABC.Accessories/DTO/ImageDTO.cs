using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.DTO;

public class ImageDTO
{
    public IFormFile File { get; set; } = null!;

    [Required]
    public required string AltText { get; set; }

    [Required]
    public required int Order { get; set; }
}