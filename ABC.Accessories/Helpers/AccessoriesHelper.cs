using System.Text.Json;
using System.Text.RegularExpressions;
using ABC.Contracts.DTO.Response;
using ABC.Accessories.Enums;

namespace ABC.Accessories.Helpers;

public partial class AccessoriesHelper(
                        ILogger<AccessoriesHelper> _logger
                    ) : IAccessoriesHelper
{
    [GeneratedRegex(@"[^a-z0-9\-]")]
    private static partial Regex MyRegex();

    private readonly JsonSerializerOptions serializationOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string SanitizeBlobName(string fileName)
    {
        int extensionPosition = fileName.LastIndexOf('.');
        string fileNameWithoutExtension = fileName[..extensionPosition];
        string extension = fileName[extensionPosition..];

        // Regex to replace any character that is not a-z, 0-9, or hyphen with a hyphen
        return $"{MyRegex().Replace(fileNameWithoutExtension.ToLower(), "-")}{extension}";
    }

    public async Task<ApiResponseDto<T>> DeserializeJsonFromFileAsync<T>(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ApiResponseDto<T>.HandleErrorResponse((int)ResponseCode.BAD_REQUEST, ["No file present"]);
        }

        // Read the file into a memory stream
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning

        try
        {
            var deserializedJson = await JsonSerializer.DeserializeAsync<T>(stream, serializationOptions);
            return ApiResponseDto<T>.HandleSuccessResponse(deserializedJson);
        }
        catch (Exception error)
        {
            _logger.LogError("Error deserializing the JSON:\n {error}", error);
            return ApiResponseDto<T>.HandleErrorResponse((int)ResponseCode.ERROR, ["Error deserializing JSON."]);

        }
    }


}