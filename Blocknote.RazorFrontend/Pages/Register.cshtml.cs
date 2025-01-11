using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Hasher;
using Blocknote.RazorFrontend.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

public class Register : PageModel
{
    private readonly IUserService _service;
    private readonly IHashService _hasher;
    [BindProperty] public RegisterViewModel ViewModel { get; set; } = new();
    
    public Register (IUserService service, IHashService hasher)
    {
        _service = service;
        _hasher = hasher;
    }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            var result = await _service.Register(ViewModel.Username, _hasher.Compute(ViewModel.Password));
            return result ? RedirectToPage("Login") : RedirectToPage("Register");
        }
        catch
        {
            return RedirectToPage("Register");
        }
    }
}