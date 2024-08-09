using ClassSurvey.Models;

namespace ClassSurvey.ViewModels;

public class AnswerVM
{
    public int QuestionId { get; set; }
    public int? OptionId { get; set; }
    public string IpAddress { get; set; } = null!;
    public string? ResponseText { get; set; }
    public Option? SelectedOption { get; set; }
    public bool IsFreeText { get; set; }
}
