using Blocknote.RazorFrontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

[Authorize]
public class Index : PageModel
{

    [BindProperty] public NoteViewModel ViewModel { get; set; } = new();
    
    public void OnGet()
    {
       
    }

    public void OnPost()
    {
        ViewModel = new();
    }
}