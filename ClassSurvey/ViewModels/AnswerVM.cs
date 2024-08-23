using ClassSurvey.Models;
using System.ComponentModel.DataAnnotations;

namespace ClassSurvey.ViewModels;

public class AnswerVM
{
    public int QuestionId { get; set; }
    public int? OptionId { get; set; }
    public string? ResponseText { get; set; }
    public Option SelectedOption { get; set; } = null!;
    public bool IsFreeText { get; set; }
}
