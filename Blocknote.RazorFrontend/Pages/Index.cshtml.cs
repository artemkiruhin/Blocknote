using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Jwt;
using Blocknote.RazorFrontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blocknote.RazorFrontend.Pages;

[Authorize]
public class Index : PageModel
{
    private readonly INoteService _noteService;
    private readonly IJwtService _jwtService;

    [BindProperty] public List<NoteViewModel> Notes { get; set; } = new();
    
    
    public Index (INoteService noteService, IJwtService jwtService)
    {
        _noteService = noteService;
        _jwtService = jwtService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var token = Request.Cookies["jwt"];
            var userId = _jwtService.GetUserId(token);
            var notes = await _noteService.GetAllAsync(userId);
            Notes = notes.Select(x => new NoteViewModel
            {
                Content = x.Content ?? "",
                Title = x.Title,
                Subtitle = x.Subtitle ?? ""
            }).ToList();
            
            return Page();
        }
        catch
        {
            return RedirectToPage("Login");
        }
    }

    public void OnPost()
    {
        Notes = new();
    }
}