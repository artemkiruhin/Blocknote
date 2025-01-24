namespace Blocknote.Core.Models.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public DateTime RegisteredAt { get; set; }
    public List<NoteDto> Notes { get; set; }
}