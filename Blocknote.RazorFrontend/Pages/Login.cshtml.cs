using Blocknote.RazorFrontend.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

public class Login : PageModel
{
    [BindProperty] public LoginViewModel ViewModel { get; set; } = new();
    public void OnGet()
    {
        
    }
}