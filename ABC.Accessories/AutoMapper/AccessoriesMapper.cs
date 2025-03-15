using AutoMapper;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.Models;
using ABC.Accessories.Models.MongoDb;
using ABC.Accessories.DTO;

namespace ABC.Accessories.AutoMapper;

public class AccessoriesMapper : Profile
{
    public AccessoriesMapper()
    {
        CreateMap<AddAccessoryDTO, Accessory>();
        CreateMap<AddAccessoryDTO, AccessoryExtras>();
        CreateMap<AddAccessoryBaseDTO, AccessoryBase>();
        CreateMap<AddAccessoryBaseDTO, AccessoryBaseExtras>();
        CreateMap<ImageDTO, ItemImage>();
        CreateMap<AddSellerDTO, Seller>();
        CreateMap<AddCategoryDTO, Category>();
        CreateMap<AddBrandDTO, Brand>();
        CreateMap<AddDeviceModelDTO, DeviceModel>();

    }
}