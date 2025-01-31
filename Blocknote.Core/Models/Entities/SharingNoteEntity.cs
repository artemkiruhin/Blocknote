using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Sharing;

namespace Blocknote.Core.Models.Entities;

public class SharingNoteEntity
{
    public Guid Id { get; set; }
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public required string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CloseAt { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual NoteEntity Note { get; set; } = null!;
    
    public static SharingNoteEntity Create(Guid noteId, Guid userId, DateTime closeAt, SharingType sharingType = SharingType.All)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            NoteId = noteId,
            UserId = userId,
            CloseAt = closeAt,
            CreatedAt = DateTime.UtcNow,
            Type = nameof(sharingType)
        };
    }
}