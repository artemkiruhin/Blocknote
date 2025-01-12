using Blocknote.Core.Models.Entities;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Jwt;
using Blocknote.RazorFrontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blocknote.RazorFrontend.Pages;

public class Index : PageModel
{
    private readonly INoteService _noteService;
    private readonly IJwtService _jwtService;

    [BindProperty] public List<NoteViewModel> Notes { get; set; } = new();
    [BindProperty] public NoteViewModel AddViewModel { get; set; } = new();
    
    
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

    public async Task<IActionResult> OnPost()
    {
        try
        {
            var userId = _jwtService.GetUserId(Request.Cookies["jwt"]);
            Console.WriteLine(userId);
            var result = await _noteService.CreateAsync(AddViewModel.Title, AddViewModel.Subtitle, AddViewModel.Content,
                userId);
            AddViewModel = new();
            Notes = [];
            return RedirectToPage(); 
            
        }
        catch (Exception e)
        {
            return await OnGetAsync();
        }
    }
}