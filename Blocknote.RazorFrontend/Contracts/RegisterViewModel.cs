using System.ComponentModel.DataAnnotations;

namespace Blocknote.RazorFrontend.Contracts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    public string Username { get; set; } = "";
    
    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; } = "";
    
    
    [Required(ErrorMessage = "Повторите введенный пароль")]
    public string ConfirmPassword { get; set; } = "";
}