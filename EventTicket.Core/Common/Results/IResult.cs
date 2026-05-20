namespace EventTicket.Core.Common.Results;

public interface IResult
{
    bool Success { get; }
    string Message { get; }
}

public interface IDataResult<T> : IResult
{
    T? Data { get; }
}
