namespace ClassSurvey.Models;

public class QuestionResult
{
    public StatusCode StatusCode { get; set; }
    public List<Question>? Questions { get; set; }
    public string? Message { get; set; }
}
