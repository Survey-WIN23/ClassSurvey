using ClassSurvey.Models;

namespace ClassSurvey.ViewModels;

public class QuestionVM
{
    public Question Question { get; set; } = null!;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
