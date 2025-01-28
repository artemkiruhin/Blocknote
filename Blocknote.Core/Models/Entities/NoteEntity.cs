namespace Blocknote.Core.Models.Entities;

public class NoteEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;
    public virtual ICollection<SharingNoteEntity> Sharings { get; set; } = new List<SharingNoteEntity>();
    
    public static NoteEntity Create(string title, string? subtitle, string? content, Guid userId)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Title = title,
            Subtitle = subtitle,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
    }
}