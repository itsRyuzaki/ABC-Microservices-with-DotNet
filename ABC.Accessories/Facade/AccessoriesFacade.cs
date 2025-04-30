using ABC.Accessories.Constants;
using ABC.Accessories.DTO.Request;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Enums;
using ABC.Accessories.Helpers;
using ABC.Accessories.Models;
using ABC.Accessories.Models.MongoDb;
using ABC.Accessories.Services;
using ABC.Accessories.Services.Blob;
using AutoMapper;

namespace ABC.Accessories.Facade;

public class AccessoriesFacade(
                    IMapper _mapper,
                    IAccessoriesService _accessoriesService,
                    IBlobService _blobService,
                    IAccessoriesHelper _accessoriesHelper,
                    ILogger<AccessoriesFacade> _logger
                ) : IAccessoriesFacade
{
    public async Task<ApiResponseDto<string>> AddAccessoryDetailAsync(AddAccessoryDTO payload)
    {
        var accessoryDetail = _mapper.Map<Accessory>(payload);

        _logger.LogInformation("Fetching Seller Details for accessory: {name}", payload.AccessoryBaseId);

        // get seller details from seller ids
        var sellers = await _accessoriesService
                                    .GetSellersFromIdsAsync(payload.SellerIds, payload.Type);


        if (sellers != null && sellers.Count != 0)
        {
            accessoryDetail.Sellers = sellers;
            accessoryDetail.AccessoryGuid = $"{payload.AccessoryBaseId}-{Guid.NewGuid()}";
            accessoryDetail.Inventory = new Inventory()
            {
                AvailableCount = payload.AvailableCount,
                TotalSold = 0
            };

            var dateTime = DateTime.UtcNow;
            accessoryDetail.CreatedDate = dateTime;
            accessoryDetail.UpdatedDate = dateTime;

            _logger.LogInformation("Saving Accessory Details for accessory: {name}", payload.AccessoryBaseId);

            var accessorySavedResponse = await _accessoriesService
                                 .AddAccessoryAsync(accessoryDetail, payload.AccessoryBaseId, payload.Type);

            if (!accessorySavedResponse.Success)
                return accessorySavedResponse;

            var extraDetails = _mapper.Map<AccessoryExtras>(payload);
            extraDetails.AccessoryGuid = accessoryDetail.AccessoryGuid;

            return await _accessoriesService.AddAccessoryExtrasAsync(extraDetails, payload.Type);

        }
        else
        {
            _logger.LogError("Invalid SellerIds for accessory: {name}", payload.AccessoryBaseId);
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.BAD_REQUEST, ["Invalid SellerIds"]);
        }
    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseDetailAsync(AddAccessoryBaseDTO payload)
    {
        var details = _mapper.Map<AccessoryBase>(payload);

        details.AccessoryBaseId = Guid.NewGuid().ToString();

        var baseSavedResponse = await _accessoriesService.AddAccessoryBaseAsync(details, payload.Type);

        if (!baseSavedResponse.Success)
            return baseSavedResponse;

        var extraDetails = _mapper.Map<AccessoryBaseExtras>(payload);
        extraDetails.AccessoryBaseId = details.AccessoryBaseId;

        return await _accessoriesService.AddAccessoryBaseExtrasAsync(extraDetails, payload.Type);
    }

    public async Task<ApiResponseDto<int>> AddSellerDetailsAsync(AddSellerDTO payload)
    {
        return await _accessoriesService.AddSellerAsync(_mapper.Map<Seller>(payload), payload.Type);
    }

    public async Task<ApiResponseDto<List<Seller>?>> GetSellersAsync(string type)
    {
        return await _accessoriesService.GetSellersAsync(type);
    }

    public async Task<ApiResponseDto<List<bool>>> AddAccessoryImagesAsync(List<IFormFile> images, IFormFile requestPayload)
    {
        var deserializedResponse = await _accessoriesHelper
                                                .DeserializeJsonFromFileAsync<AddAccessoryImagesDTO>(requestPayload);

        if (!deserializedResponse.Success || deserializedResponse.Data == null)
        {
            return new ApiResponseDto<List<bool>>()
            {
                Success = false,
                ErrorDetails = deserializedResponse.ErrorDetails
            };
        }

        var itemImagesPayload = deserializedResponse.Data;
        var type = itemImagesPayload.Type;
        var accessoryGuid = itemImagesPayload.AccessoryGuid;

        var accessory = await _accessoriesService.GetAccessoryFromGuidAsync(accessoryGuid, type);

        if (accessory == null)
        {
            return ApiResponseDto<List<bool>>.HandleErrorResponse(
                                            (int)ResponseCode.BAD_REQUEST,
                                            ["No Accessory details found for given guid"]
                                        );
        }


        for (int i = 0; i < itemImagesPayload.ItemImages.Count; i++)
        {
            itemImagesPayload.ItemImages[i].File = images[i];

        }

        return await SaveImagesToBlobAndDbAsync(itemImagesPayload, type, accessory);

    }

    private async Task<ApiResponseDto<List<bool>>> SaveImagesToBlobAndDbAsync(AddAccessoryImagesDTO itemImagesPayload, string type, Accessory accessory)
    {
        List<bool> fileSavedResponse = [];
        List<ItemImage> savedItemImages = [];


        await Parallel.ForEachAsync(
                itemImagesPayload.ItemImages,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (itemImageDTO, CancellationToken) =>
                {
                    var fileName = _accessoriesHelper.SanitizeBlobName(itemImageDTO.File.FileName);
                    var filePath = $"{BlobPath.ItemImages}/{accessory.AccessoryGuid}/{fileName}";

                    var fileSaved = await _blobService.Upload(
                                            type.ToLower(),
                                            filePath,
                                            itemImageDTO.File
                                        );
                    if (fileSaved)
                    {
                        var itemImage = _mapper.Map<ItemImage>(itemImageDTO);
                        itemImage.Source = filePath;
                        savedItemImages.Add(itemImage);

                    }
                    fileSavedResponse.Add(fileSaved);
                });


        if (savedItemImages.Count != 0)
        {
            var savedResponse = await _accessoriesService
                                            .AddImagesToAccessoryAsync(savedItemImages, accessory, type);
            if (savedResponse.Success)
            {
                return ApiResponseDto<List<bool>>.HandleSuccessResponse(fileSavedResponse);
            }
        }


        return ApiResponseDto<List<bool>>.HandleErrorResponse(
                                                            (int)ResponseCode.ERROR,
                                                            ["Error while saving images"]
                                                        );
    }

    public async Task<ApiResponseDto<int>> AddCategoryAsync(AddCategoryDTO categoryDTO)
    {

        var category = _mapper.Map<Category>(categoryDTO);
        var fileName = _accessoriesHelper.SanitizeBlobName(categoryDTO.File.FileName);

        category.Guid = Guid.NewGuid().ToString();

        var blobFilePath = $"{BlobPath.CategoryImages}/{category.Guid}/{fileName}";

        category.Source = blobFilePath;

        var dbResponse = await _accessoriesService.AddCategoryAsync(category, categoryDTO.Type);

        if (dbResponse.Success)
        {
            var fileSaved = await _blobService.Upload(
                                            categoryDTO.Type.ToLower(),
                                            blobFilePath,
                                            categoryDTO.File
                                        );

            if (!fileSaved)
            {
                await _accessoriesService.DeleteCategoryByIdAsync(dbResponse.Data, categoryDTO.Type);
                return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving category images"]);
            }
        }

        return dbResponse;

    }

    public async Task<ApiResponseDto<List<Category>?>> GetCategoriesAsync(string type)
    {
        return await _accessoriesService.GetCategoriesAsync(type);
    }

    public async Task<ApiResponseDto<int>> AddBrandAsync(AddBrandDTO brandDTO)
    {

        var brand = _mapper.Map<Brand>(brandDTO);
        var fileName = _accessoriesHelper.SanitizeBlobName(brandDTO.File.FileName);

        brand.Guid = Guid.NewGuid().ToString();

        var blobFilePath = $"{BlobPath.CategoryImages}/{brand.Guid}/{fileName}";

        brand.Source = blobFilePath;

        var dbResponse = await _accessoriesService.AddBrandAsync(brand, brandDTO.Type);

        if (dbResponse.Success)
        {
            var fileSaved = await _blobService.Upload(
                                            brandDTO.Type.ToLower(),
                                            blobFilePath,
                                            brandDTO.File
                                        );

            if (!fileSaved)
            {
                await _accessoriesService.DeleteCategoryByIdAsync(dbResponse.Data, brandDTO.Type);
                return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving brand images"]);
            }
        }

        return dbResponse;

    }

    public async Task<ApiResponseDto<List<Brand>?>> GetBrandsAsync(string type)
    {
        return await _accessoriesService.GetBrandsAsync(type);
    }

    public async Task<ApiResponseDto<int>> AddDeviceModelAsync(AddDeviceModelDTO deviceModelDTO)
    {

        var deviceModel = _mapper.Map<DeviceModel>(deviceModelDTO);
        var fileName = _accessoriesHelper.SanitizeBlobName(deviceModelDTO.File.FileName);

        deviceModel.Guid = Guid.NewGuid().ToString();

        var blobFilePath = $"{BlobPath.CategoryImages}/{deviceModel.Guid}/{fileName}";

        deviceModel.Source = blobFilePath;

        var dbResponse = await _accessoriesService.AddDeviceModelAsync(deviceModel, deviceModelDTO.Type);

        if (dbResponse.Success)
        {
            var fileSaved = await _blobService.Upload(
                                            deviceModelDTO.Type.ToLower(),
                                            blobFilePath,
                                            deviceModelDTO.File
                                        );

            if (!fileSaved)
            {
                await _accessoriesService.DeleteDeviceModelByIdAsync(dbResponse.Data, deviceModelDTO.Type);
                return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving device model images"]);
            }
        }

        return dbResponse;

    }

    public async Task<ApiResponseDto<List<DeviceModel>?>> GetDeviceModelsAsync(string type)
    {
        return await _accessoriesService.GetDeviceModelsAsync(type);
    }

    public Task<ApiResponseDto<List<CombinedAccessoryDetail>>> FilterAccessoriesAsync(FilterAccessoriesDTO requestPayload)
    {
        return _accessoriesService.FilterAccessoriesAsync(requestPayload);
    }
}