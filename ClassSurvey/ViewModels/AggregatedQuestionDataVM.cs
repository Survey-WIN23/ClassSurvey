namespace ClassSurvey.ViewModels;

public class AggregatedQuestionDataVM
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = null!;
    public Dictionary<string, int> OptionsCount { get; set; } = [];
    public string? ResponseText { get; set; }
}
