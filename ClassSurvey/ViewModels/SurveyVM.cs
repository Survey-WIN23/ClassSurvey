namespace ClassSurvey.ViewModels;

public class SurveyVM
{
    public List<AggregatedQuestionDataVM> Data { get; set; } = null!;
    public string? OverallAnalysis { get; set; }
}
