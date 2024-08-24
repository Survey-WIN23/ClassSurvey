using ClassSurvey.Models;

namespace ClassSurvey.ViewModels;

public class SignInVM
{
    public SignIn Form { get; set; } = new SignIn();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}
