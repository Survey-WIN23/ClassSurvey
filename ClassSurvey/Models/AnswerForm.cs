namespace ClassSurvey.Models;

public class AnswerForm
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public int? OptionId { get; set; }
    public string IpAddress { get; set; } = null!;
    public string? ResponseText { get; set; }
    public bool IsFreeText { get; set; }
    public Option? SelectedOption { get; set; }
    public Question? Question { get; set; }
}
