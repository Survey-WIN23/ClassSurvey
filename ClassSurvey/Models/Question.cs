namespace ClassSurvey.Models;

public class Question
{
    public int Id { get; set; }
    public string? QuestionText { get; set; }
    public QuestionType Type { get; set; }
    public bool HasFreeText { get; set; }

    public List<Option> Options { get; set; } = null!;
}

public enum QuestionType
{
    MultipleChoice,
    FreeText,
    MultipleChoiceWithFreeText
}
