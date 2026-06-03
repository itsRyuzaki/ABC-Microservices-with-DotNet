using ABC.Contracts.DTO.Response;

namespace ABC.Accessories.Helpers;

public interface IAccessoriesHelper
{
    public string SanitizeBlobName(string fileName);
    public Task<ApiResponseDto<T>> DeserializeJsonFromFileAsync<T>(IFormFile file);

}