using System.ComponentModel.DataAnnotations;

namespace App.Infrastructure.Services.Identity;
public class PasswordResetOptions
{
    [Required] public string ResetLinkBaseUrl { get; set; } = default!;
    [Required] public string EmailSubject { get; set; } = "Reset your password";
    public string EmailGreeting { get; set; } = "Hello";
    public string EmailBody { get; set; } = "Click the button below to reset your password.";
    public string ButtonText { get; set; } = "Reset Password";
}
