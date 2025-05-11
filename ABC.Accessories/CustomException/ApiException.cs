using ABC.Accessories.Enums;

namespace ABC.Accessories.CustomException;

public class ApiException : Exception
{
    public ResponseCode ErrorCode { get; }

    public ApiException(ResponseCode responseCode, string message) : base(message)
    {
        ErrorCode = responseCode;
    }

    public ApiException(
            ResponseCode responseCode, 
            string message, 
            Exception innerException
        ) : base(message, innerException)
    {
        ErrorCode = responseCode;
    }
}