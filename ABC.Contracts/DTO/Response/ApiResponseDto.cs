namespace ABC.Contracts.DTO.Response;


public class ErrorDetails
{
    public required int Code { get; set; }
    public string[] Details { get; set; } = [];
}
public class ApiResponseDto<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }

    public ErrorDetails? ErrorDetails { get; set; }
    public static ApiResponseDto<T> HandleErrorResponse(int code, string[] details)
    {
        return new ApiResponseDto<T>()
        {
            Success = false,
            ErrorDetails = new ErrorDetails()
            {
                Code = code,
                Details = details
            }
        };
    }

    public static ApiResponseDto<T> HandleSuccessResponse(T? data = default)
    {
        return new ApiResponseDto<T>()
        {
            Success = true,
            Data = data
        };
    }
}

// fallback for no type
public class ApiResponseDto : ApiResponseDto<string>
{

}