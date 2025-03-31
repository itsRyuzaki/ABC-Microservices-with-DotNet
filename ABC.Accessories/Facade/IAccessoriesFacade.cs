using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;

namespace ABC.Accessories.Facade;

public interface IAccessoriesFacade
{

    public Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoryDTO requestPayload);

    public Task<ApiResponseDto<string>> AddAccessoryBaseDetailAsync(AddAccessoryBaseDTO payload);

    public Task<ApiResponseDto<int>> AddSellerDetailsAsync(AddSellerDTO payload);

    public Task<ApiResponseDto<List<Seller>?>> GetSellersAsync(string type);

    public Task<ApiResponseDto<List<bool>>> AddAccessoryImagesAsync(List<IFormFile> images, IFormFile requestPayload);

    public Task<ApiResponseDto<int>> AddCategoryAsync(AddCategoryDTO categoryDTO);
    public Task<ApiResponseDto<List<Category>?>> GetCategoriesAsync(string type);

    public Task<ApiResponseDto<int>> AddBrandAsync(AddBrandDTO brandDTO);

    public Task<ApiResponseDto<List<Brand>?>> GetBrandsAsync(string type);

    public Task<ApiResponseDto<int>> AddDeviceModelAsync(AddDeviceModelDTO deviceModelDTO);

    public Task<ApiResponseDto<List<DeviceModel>?>> GetDeviceModelsAsync(string type);

}