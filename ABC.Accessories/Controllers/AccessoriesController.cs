using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Facade;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Accessories.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class AccessoriesController(IAccessoriesFacade _accessoriesFacade) : ControllerBase
{
    [HttpGet("Health", Name = "GetHealth")]
    public string GetHealthStatus()
    {
        return "Accessories microservice up and running!!";
    }

    private ObjectResult GetStatusCode<T>(ApiResponseDto<T> response, ResponseCode successCode)
    {
        return StatusCode(
                   response.Success ? (int)successCode : (response.ErrorDetails?.Code ?? (int)ResponseCode.ERROR),
                   response
               );
    }

    [HttpPost("accessory-base")]
    public async Task<IActionResult> AddAccessoryBase(AddAccessoryBaseDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryBaseDetailAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddAccessory(AddAccessoryDTO payload)
    {
        var response = await _accessoriesFacade.AddAccessoryDetailAsync(payload);

        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);

    }

    [HttpPost("filter")]
    public async Task<IActionResult> GetFilteredAccessories(FilterAccessoriesDTO payload)
    {
        var response = await _accessoriesFacade.FilterAccessoriesAsync(payload);

        return GetStatusCode(response, ResponseCode.SUCCESS);

    }

    [HttpPost("details/{accessoryId}")]
    public async Task<IActionResult> GetAccessoryDetailsById([FromRoute] string accessoryId, BasePayloadDTO payload)
    {
        var response = await _accessoriesFacade.GetAccessoryDetailsByIdAsync(accessoryId, payload);
        return GetStatusCode(response, ResponseCode.SUCCESS);
    }

    [HttpPost("images")]
    public async Task<IActionResult> AddAccessoryImages(List<IFormFile> images, IFormFile requestPayload)
    {
        var response = await _accessoriesFacade.AddAccessoryImagesAsync(images, requestPayload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    [HttpPost("sellers")]
    public async Task<IActionResult> AddSeller(AddSellerDTO payload)
    {
        var response = await _accessoriesFacade.AddSellerDetailsAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);
    }

    [HttpGet("sellers")]
    public async Task<IActionResult> GetSellers(string type)
    {
        var response = await _accessoriesFacade.GetSellersAsync(type);
        return GetStatusCode(response, ResponseCode.SUCCESS);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> AddCategory([FromForm] AddCategoryDTO payload)
    {
        var response = await _accessoriesFacade.AddCategoryAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);

    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(string type)
    {
        var response = await _accessoriesFacade.GetCategoriesAsync(type);
        return GetStatusCode(response, ResponseCode.SUCCESS);

    }

    [HttpPost("brands")]
    public async Task<IActionResult> AddBrand([FromForm] AddBrandDTO payload)
    {
        var response = await _accessoriesFacade.AddBrandAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);

    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands(string type)
    {
        var response = await _accessoriesFacade.GetBrandsAsync(type);
        return GetStatusCode(response, ResponseCode.SUCCESS);

    }

    [HttpPost("device-models")]
    public async Task<IActionResult> AddDeviceModel([FromForm] AddDeviceModelDTO payload)
    {
        var response = await _accessoriesFacade.AddDeviceModelAsync(payload);
        return GetStatusCode(response, ResponseCode.SUCCESS_CREATED);

    }

    [HttpGet("device-models")]
    public async Task<IActionResult> GetDeviceModels(string type)
    {
        var response = await _accessoriesFacade.GetDeviceModelsAsync(type);
        return GetStatusCode(response, ResponseCode.SUCCESS);
    }


}
