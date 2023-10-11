using Core.Enums;

namespace Core.DTO;

public class Result<T>
{
    public ResultType StatusCode { get; set; }
    public string? Error { get; set; }
    public T View { get; set; }
}
