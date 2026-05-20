namespace EventTicket.Core.Common.Results;

public class Result : IResult
{
    public bool Success { get; }
    public string Message { get; }
    public string? ErrorCode { get; } 

    protected Result(bool success, string message, string? errorCode = null)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
    }

    public static Result Ok(string message = "İşlem başarılı.")
        => new(true, message);

    public static Result Fail(string message, string? errorCode = null)
        => new(false, message, errorCode);
}

public class DataResult<T> : Result, IDataResult<T>
{
    public T? Data { get; }

    private DataResult(bool success, T? data, string message, string? errorCode = null)
        : base(success, message, errorCode)
    {
        Data = data;
    }

    public static DataResult<T> Ok(T data, string message = "İşlem başarılı.")
        => new(true, data, message);

    public static new DataResult<T> Fail(string message, string? errorCode = null)
        => new(false, default, message, errorCode);
}