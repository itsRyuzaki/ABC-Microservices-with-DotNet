using ABC.Accessories.Constants;
using ABC.Accessories.Data;
using ABC.Accessories.DTO.Response;
using ABC.Accessories.Models;
using ABC.Accessories.Enums;
using Microsoft.EntityFrameworkCore;
using ABC.Accessories.Services.MongoDb;
using ABC.Accessories.Models.MongoDb;
using MongoDB.Driver;

namespace ABC.Accessories.Services;
public class AccessoriesService : IAccessoriesService
{

    private readonly Dictionary<string, AccessoriesDataContext> _contextMap = [];
    private readonly Dictionary<string, IMongoCollection<AccessoryExtras>> _accExtrasCollectionMap = [];
    private readonly Dictionary<string, IMongoCollection<AccessoryBaseExtras>> _baseExtrasCollectionMap = [];

    private readonly ILogger _logger;

    public AccessoriesService(
                ComputersDataContext computersDataContext,
                MobilesDataContext mobilesDataContext,
                ILogger<AccessoriesService> logger,
                IMongoDbService mongoDbService
            )
    {
        _accExtrasCollectionMap = mongoDbService.GetAccExtrasCollectionMap();
        _baseExtrasCollectionMap = mongoDbService.GetBaseExtrasCollectionMap();

        _contextMap.Add(AccessoriesType.Mobile, mobilesDataContext);
        _contextMap.Add(AccessoriesType.Computer, computersDataContext);
        _logger = logger;

    }


    public async Task<ApiResponseDto<string>> AddAccessoryAsync(
                                                            Accessory accessory,
                                                            string accessoryBaseId,
                                                            string type
                                                        )
    {
        try
        {
            var accessoryBaseDetail = await _contextMap[type].AccessoryBase
                            .SingleOrDefaultAsync(x => x.AccessoryBaseId == accessoryBaseId);

            if (accessoryBaseDetail == null)
            {
                _logger.LogError("No base details found for baseId: {baseId}", accessoryBaseId);
                throw new Exception($"No base details found for baseId: {accessoryBaseId}");
            }
            else
            {
                accessoryBaseDetail.Accessories.Add(accessory);
                await _contextMap[type].SaveChangesAsync();
                _logger.LogInformation("Saved details for accessory: {name}", accessoryBaseId);
            }

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for accessory: {name}. See error stack below: \n {error}",
                        accessoryBaseId,
                        error.ToString()
                    );
            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving accessory details."]);
        }

        return ApiResponseDto.HandleSuccessResponse(accessory.AccessoryGuid);

    }

    public async Task<ApiResponseDto<string>> AddAccessoryExtrasAsync(AccessoryExtras accessoryExtras, string type)
    {
        try
        {
            await _accExtrasCollectionMap[type].InsertOneAsync(accessoryExtras);

            _logger.LogInformation("Saved extra details for Accessory: {name}", accessoryExtras.AccessoryGuid);
            return ApiResponseDto.HandleSuccessResponse("Accessory Extra Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving extra details for accessory: {name}. See error stack below: \n {error}",
                        accessoryExtras.AccessoryGuid,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving extra details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseAsync(AccessoryBase accessoryBase, string type)
    {
        try
        {
            _contextMap[type].AccessoryBase.Add(accessoryBase);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved base details for Accessory: {name}", accessoryBase.Name);
            return ApiResponseDto.HandleSuccessResponse("Base Accessory Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving base details for accessory: {name}. See error stack below: \n {error}",
                        accessoryBase.Name,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving base details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<string>> AddAccessoryBaseExtrasAsync(AccessoryBaseExtras baseExtras, string type)
    {
        try
        {
            await _baseExtrasCollectionMap[type].InsertOneAsync(baseExtras);

            _logger.LogInformation("Saved extra base details for Accessory: {name}", baseExtras.AccessoryBaseId);
            return ApiResponseDto.HandleSuccessResponse("Extra Accessory Base Added");

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving extra base details for accessory: {name}. See error stack below: \n {error}",
                        baseExtras.AccessoryBaseId,
                        error.ToString()
                    );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving extra base details for accessory."]);
        }


    }

    public async Task<ApiResponseDto<int>> AddSellerAsync(Seller seller, string type)
    {

        try
        {
            _contextMap[type].Sellers.Add(seller);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for seller: {name}", seller.Name);
            return ApiResponseDto<int>.HandleSuccessResponse(seller.Id);

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for seller: {name}. See error stack below: \n {error}",
                        seller.Name,
                        error.ToString()
                    );

            return ApiResponseDto<int>.HandleErrorResponse(
                            (int)ResponseCode.ERROR,
                            ["Error while saving seller details."]
                        );
        }


    }

    public async Task<ApiResponseDto<List<Seller>?>> GetSellersAsync(string type)
    {
        try
        {
            _logger.LogInformation("Fetching sellers for type: {type}", type);

            var list = await _contextMap[type].Sellers.ToListAsync();

            return ApiResponseDto<List<Seller>?>.HandleSuccessResponse(list);

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching Sellers for type: {type}. See error stack below: \n {error}",
                type,
                error.ToString()
            );
            return ApiResponseDto<List<Seller>?>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while fetching Sellers"]);
        }
    }

    public async Task<List<Seller>> GetSellersFromIdsAsync(int[] sellerIds, string type)
    {
        try
        {
            _logger.LogInformation("Fetching seller details...");

            return await _contextMap[type].Sellers
                                            .Where(seller => sellerIds.Contains(seller.Id))
                                            .ToListAsync();
        }
        catch (Exception error)
        {
            _logger.LogError("Error while fetching seller details. See error stack below: \n {error}", error.ToString());
            return [];
        }
    }

    public async Task<Accessory?> GetAccessoryFromGuidAsync(string accessoryGuid, string type)
    {
        try
        {
            _logger.LogInformation("Fetching Accessory details from Guid: {guid}", accessoryGuid);

            return await _contextMap[type].Accessories
                                             .FirstOrDefaultAsync(x => x.AccessoryGuid == accessoryGuid);

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching Accessory details from Guid: {guid}. See error stack below: \n {error}",
                accessoryGuid,
                error.ToString()
            );
            return null;
        }
    }

    public async Task<ApiResponseDto<string>> AddImagesToAccessoryAsync(List<ItemImage> itemImages, Accessory accessory, string type)
    {
        try
        {
            accessory.Images.AddRange(itemImages);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved images for Accessory: {guid}", accessory.AccessoryGuid);
            return ApiResponseDto.HandleSuccessResponse("Added Images to Accessory Details");

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while saving images for accessory: {guid}. See error stack below: \n {error}",
                accessory.AccessoryGuid,
                error.ToString()
            );

            return ApiResponseDto.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving images for accessory."]);
        }
    }

    public async Task<ApiResponseDto<int>> AddCategoryAsync(Category category, string type)
    {
        try
        {
            _contextMap[type].Category.Add(category);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for Category: {name}", category.Name);
            return ApiResponseDto<int>.HandleSuccessResponse(category.Id);

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for Category: {name}. See error stack below: \n {error}",
                        category.Name,
                        error.ToString()
                    );

            return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving category details."]);
        }
    }


    public async Task<ApiResponseDto<bool>> DeleteCategoryByIdAsync(int categoryId, string type)
    {
        try
        {
            int deletedRows = await _contextMap[type].Category
                                        .Where(category => category.Id == categoryId).ExecuteDeleteAsync();


            _logger.LogInformation("Removed details for Category: {categoryId}", categoryId);

            if (deletedRows > 0)
            {
                return ApiResponseDto<bool>.HandleSuccessResponse(true);
            }
            else
            {
                return ApiResponseDto<bool>.HandleErrorResponse(
                                                (int)ResponseCode.NOT_FOUND,
                                                ["No Record found for given category"]
                                            );
            }

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while deleting details for Category: {categoryId}. See error stack below: \n {error}",
                        categoryId,
                        error.ToString()
                    );

            return ApiResponseDto<bool>
                    .HandleErrorResponse((int)ResponseCode.ERROR, ["Error while deleting category details."]);
        }
    }

    public async Task<ApiResponseDto<List<Category>?>> GetCategoriesAsync(string type)
    {
        try
        {
            _logger.LogInformation("Fetching Categories for type: {type}", type);

            var list = await _contextMap[type].Category.ToListAsync();

            return ApiResponseDto<List<Category>?>.HandleSuccessResponse(list);

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching Categories for type: {type}. See error stack below: \n {error}",
                type,
                error.ToString()
            );
            return ApiResponseDto<List<Category>?>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while fetching Categories"]);
        }
    }


    public async Task<ApiResponseDto<int>> AddBrandAsync(Brand brand, string type)
    {
        try
        {
            _contextMap[type].Brands.Add(brand);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for Brand: {name}", brand.Name);
            return ApiResponseDto<int>.HandleSuccessResponse(brand.Id);

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for Brand: {name}. See error stack below: \n {error}",
                        brand.Name,
                        error.ToString()
                    );

            return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving category details."]);
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteBrandByIdAsync(int brandId, string type)
    {
        try
        {
            int deletedRows = await _contextMap[type].Brands
                                        .Where(brand => brand.Id == brandId).ExecuteDeleteAsync();


            _logger.LogInformation("Removed details for Brand: {brandId}", brandId);

            if (deletedRows > 0)
            {
                return ApiResponseDto<bool>.HandleSuccessResponse(true);
            }
            else
            {
                return ApiResponseDto<bool>.HandleErrorResponse(
                                                (int)ResponseCode.NOT_FOUND,
                                                ["No Record found for given brand"]
                                            );
            }

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while deleting details for Brand: {brandId}. See error stack below: \n {error}",
                        brandId,
                        error.ToString()
                    );

            return ApiResponseDto<bool>
                    .HandleErrorResponse((int)ResponseCode.ERROR, ["Error while deleting brand details."]);
        }
    }


    public async Task<ApiResponseDto<List<Brand>?>> GetBrandsAsync(string type)
    {
        try
        {
            _logger.LogInformation("Fetching Brands for type: {type}", type);

            var list = await _contextMap[type].Brands.ToListAsync();

            return ApiResponseDto<List<Brand>?>.HandleSuccessResponse(list);

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching Brands for type: {type}. See error stack below: \n {error}",
                type,
                error.ToString()
            );
            return ApiResponseDto<List<Brand>?>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while fetching Brands for given type."]);
        }
    }

    public async Task<ApiResponseDto<int>> AddDeviceModelAsync(DeviceModel deviceModel, string type)
    {
        try
        {
            _contextMap[type].DeviceModel.Add(deviceModel);
            await _contextMap[type].SaveChangesAsync();

            _logger.LogInformation("Saved details for DeviceModel: {name}", deviceModel.Name);
            return ApiResponseDto<int>.HandleSuccessResponse(deviceModel.Id);

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while saving details for DeviceModel: {name}. See error stack below: \n {error}",
                        deviceModel.Name,
                        error.ToString()
                    );

            return ApiResponseDto<int>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error while saving Device Model details."]);
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteDeviceModelByIdAsync(int deviceModelId, string type)
    {
        try
        {
            int deletedRows = await _contextMap[type].DeviceModel
                                        .Where(deviceModel => deviceModel.Id == deviceModelId).ExecuteDeleteAsync();


            _logger.LogInformation("Removed details for Device Model: {deviceModelId}", deviceModelId);

            if (deletedRows > 0)
            {
                return ApiResponseDto<bool>.HandleSuccessResponse(true);
            }
            else
            {
                return ApiResponseDto<bool>.HandleErrorResponse(
                                                (int)ResponseCode.NOT_FOUND,
                                                ["No Record found for given device model"]
                                            );
            }

        }
        catch (Exception error)
        {
            _logger.LogError(
                        "Error while deleting details for Device Model: {deviceModelId}. See error stack below: \n {error}",
                        deviceModelId,
                        error.ToString()
                    );

            return ApiResponseDto<bool>
                    .HandleErrorResponse((int)ResponseCode.ERROR, ["Error while deleting device model details."]);
        }
    }


    public async Task<ApiResponseDto<List<DeviceModel>?>> GetDeviceModelsAsync(string type)
    {
        try
        {
            _logger.LogInformation("Fetching DeviceModels for type: {type}", type);

            var list = await _contextMap[type].DeviceModel.ToListAsync();
            return ApiResponseDto<List<DeviceModel>?>.HandleSuccessResponse(list);

        }
        catch (Exception error)
        {
            _logger.LogError(
                "Error while fetching DeviceModels for type: {type}. See error stack below: \n {error}",
                type,
                error.ToString()
            );
            return ApiResponseDto<List<DeviceModel>?>.HandleErrorResponse(
                                                            (int)ResponseCode.ERROR,
                                                            ["Error while fetching device models"]
                                                        );
        }
    }



}