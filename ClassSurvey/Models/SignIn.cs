using System.ComponentModel.DataAnnotations;

namespace ClassSurvey.Models;

public class SignIn
{
    public int Id { get; set; }

    [Display(Name = "Email", Prompt = "Enter your email.", Order = 0)]
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "******", Order = 1)]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(2, ErrorMessage = "Not valid.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
