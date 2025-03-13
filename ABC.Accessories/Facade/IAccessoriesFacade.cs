using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;

namespace ABC.Accessories.Facade;

public interface IAccessoriesFacade
{

    public Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoryDTO requestPayload);

    public Task<ApiResponseDto<string>> AddAccessoryBaseDetailAsync(AddAccessoryBaseDTO payload);
    
    public Task<ApiResponseDto<string>> AddSellerDetailsAsync(AddSellerDTO payload);

    public Task<ApiResponseDto<List<bool>>> AddAccessoryImagesAsync(List<IFormFile> images, IFormFile requestPayload);

    public Task<ApiResponseDto<int>> AddCategoryAsync(AddCategoryDTO categoryDTO);
    public Task<ApiResponseDto<List<Category>?>> GetCategoriesAsync(string type);



}