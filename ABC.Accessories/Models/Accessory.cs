using System.ComponentModel.DataAnnotations;

namespace ABC.Accessories.Models;

public class Accessory
{
    public int Id { get; set; }

    public AccessoryBase AccessoryBase { get; set; } = null!;

    public Inventory Inventory { get; set; } = null!;

    public List<Seller> Sellers { get; set; } = [];

    public List<ItemImage> Images { get; set; } = [];

    [Required]
    public required string AccessoryGuid { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public decimal SellerPrice { get; set; }

    [Required]
    public decimal AbcPrice { get; set; }

    [Required]
    public decimal OriginalPrice { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

}
