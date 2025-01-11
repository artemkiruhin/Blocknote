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
    private readonly IConfiguration _configuration;
    [BindProperty] public LoginViewModel ViewModel { get; set; } = new();
    
    public Login(IUserService service, IHashService hasher, IJwtService jwtService, IConfiguration configuration)
    {
        _service = service;
        _hasher = hasher;
        _jwtService = jwtService;
        _configuration = configuration;
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
            
            if (result == string.Empty) return RedirectToPage("Login");

            HttpContext.Response.Cookies.Append("jwt", result, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["jwt:expires"]))
            });
            return RedirectToPage("Index");
        }
        catch (Exception e)
        {
            return RedirectToPage("Login");
        }
    }
}