namespace Health_Care_System.Domain.Entities.Common.Results;

public class GenericResult <T> : Result
{
    public T? Data {get; set;}
    
    public GenericResult(bool isSuccess , T? data,string? error) : base(isSuccess, error)
    {
        Data = data;
    }
    
    public static GenericResult<T> Success(T data) 
        => new GenericResult<T>(true, data, null);
    
    public static GenericResult<T> Failure(string error)
     => new GenericResult<T>(false, default, error);
}