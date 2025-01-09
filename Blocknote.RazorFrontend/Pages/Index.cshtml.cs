using Blocknote.RazorFrontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

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