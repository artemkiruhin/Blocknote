using System.ComponentModel.DataAnnotations;

namespace Blocknote.RazorFrontend.Contracts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    public string Username { get; set; } = "";
    
    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; } = "";
}