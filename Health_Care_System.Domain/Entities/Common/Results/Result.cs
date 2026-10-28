namespace Health_Care_System.Domain.Entities.Common.Results;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    public Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new Result(true, null);
    
    public static Result Failure(string error)
        => new Result(false, error);

}