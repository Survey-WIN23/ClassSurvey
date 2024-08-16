namespace ClassSurvey.Models;

public enum StatusCode
{
    OK = 200,
    ERROR = 400,
    NOT_FOUND = 404,
    EXISTS = 409
}
public class ResponseResult
{
    public StatusCode StatusCode { get; set; }
    public object? ContentResult { get; set; }
    public string? Message { get; set; }

}
