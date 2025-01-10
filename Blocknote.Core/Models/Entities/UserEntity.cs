namespace Blocknote.Core.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegisteredAt { get; set; }
    public virtual ICollection<NoteEntity> Notes { get; set; } = new List<NoteEntity>();
    public static UserEntity Create(string username, string passwordHash)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = passwordHash,
            RegisteredAt = DateTime.UtcNow
        };
    }
}