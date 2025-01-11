using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Hasher;
using Blocknote.Core.Services.Jwt;
using Blocknote.RazorFrontend.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

public class Login : PageModel
{
    private readonly IUserService _service;
    private readonly IHashService _hasher;
    private readonly IJwtService _jwtService;
    [BindProperty] public LoginViewModel ViewModel { get; set; } = new();
    
    public Login(IUserService service, IHashService hasher, IJwtService jwtService)
    {
        _service = service;
        _hasher = hasher;
        _jwtService = jwtService;
    }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var result = await _service.Login(ViewModel.Username, _hasher.Compute(ViewModel.Password));
            ViewModel = new();
            return result != string.Empty ? RedirectToPage("Index") : RedirectToPage("Login");
        }
        catch (Exception e)
        {
            return RedirectToPage("Login");
        }
    }
}