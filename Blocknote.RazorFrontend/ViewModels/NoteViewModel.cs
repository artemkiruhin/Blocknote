using System.ComponentModel.DataAnnotations;

namespace Blocknote.RazorFrontend.ViewModels;

public class NoteViewModel
{
    [Required(ErrorMessage = "Нужно дать название заметке")]
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string Content { get; set; } = "";
}