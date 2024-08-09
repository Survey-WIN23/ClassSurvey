namespace ClassSurvey.Models;

public class AnswerForm
{
    public int QuestionId { get; set; }
    public int? OptionId { get; set; }
    public string? ResponseText { get; set; }
    public Option? SelectedOption { get; set; }
    public bool IsFreeText { get; set; }
}
