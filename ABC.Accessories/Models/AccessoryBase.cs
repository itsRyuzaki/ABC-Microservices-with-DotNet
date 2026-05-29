using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class AccessoryBase
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string AccessoryBaseId { get; set; }
    public List<Accessory> Accessories { get; set; } = [];

    [Required]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Required]
    public int DeviceModelId { get; set; }
    public DeviceModel DeviceModel { get; set; } = null!;

    [Required]
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}