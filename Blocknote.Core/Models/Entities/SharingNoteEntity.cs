using Blocknote.Core.Models.Enums;

namespace Blocknote.Core.Models.Entities;

public class SharingNoteEntity
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public Guid NoteId { get; set; }
    public Guid UserId { get; set; }
    public required string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime CloseAt { get; set; }
    
    public virtual UserEntity User { get; set; } = null!;
    public virtual NoteEntity Note { get; set; } = null!;
    
    public static SharingNoteEntity Create(Guid? id, Guid noteId, string code, Guid userId, DateTime closeAt, SharingType sharingType = SharingType.Public)
    {
        return new()
        {
            Id = id ?? Guid.NewGuid(),
            Code = code,
            NoteId = noteId,
            UserId = userId,
            CloseAt = closeAt,
            CreatedAt = DateTime.UtcNow,
            Type = sharingType switch
            {
                SharingType.Public => nameof(SharingType.Public).ToLower(),
                SharingType.Registered => nameof(SharingType.Registered).ToLower(),
                _ => throw new ArgumentException()
            }
        };
    }
}