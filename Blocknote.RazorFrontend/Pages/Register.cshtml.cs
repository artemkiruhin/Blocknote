using Blocknote.RazorFrontend.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

public class Register : PageModel
{
    [BindProperty] public RegisterViewModel ViewModel { get; set; } = new();
    
    public void OnGet()
    {
        
    }

    public void OnPost()
    {
        
    }
}