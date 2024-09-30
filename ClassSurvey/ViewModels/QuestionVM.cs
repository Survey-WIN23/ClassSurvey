using ClassSurvey.Models;

namespace ClassSurvey.ViewModels;

public class QuestionVM
{
    public Question? Question { get; set; } 
    public List<Question> Questions { get; set; } = new List<Question>();
    public List<Option> Options { get; set; } = new List<Option>();
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public List<AnswerVM> UserAnswers { get; set; } = new List<AnswerVM>();

    // Om det är en sista fråga och behöver visa "Submit" istället för "Next"
    public bool IsLastQuestion => CurrentPage >= TotalPages;
}
