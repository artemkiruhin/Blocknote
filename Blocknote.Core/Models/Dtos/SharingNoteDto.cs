namespace Blocknote.Core.Models.Dtos;

public class SharingNoteDto
{
    public Guid Id { get; set; }
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
    public DateTime CloseAt { get; set; }
    public string AuthorUsername { get; set; }
    public string AccessType { get; set; }
}