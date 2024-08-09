namespace ClassSurvey.Models;

public class Option
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Value { get; set; } = null!;
}
