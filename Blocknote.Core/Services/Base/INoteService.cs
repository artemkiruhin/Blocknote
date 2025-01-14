using Blocknote.Core.Models.Dtos;

namespace Blocknote.Core.Services.Base;

public interface INoteService
{
    Task<NoteDto?> GetInfoAsync(Guid userId, Guid noteId);
    Task<IEnumerable<NoteDto>> GetByFullContentAsync(string fullContent);
    Task<bool> CreateAsync(string title, string? subtitle, string? content, Guid userId);
    Task<bool> EditAsync(Guid userId, Guid noteId, string title, string? subtitle, string? content);
    Task<bool> DeleteAsync(Guid userId, Guid noteId);
    Task<IEnumerable<NoteDto>> GetAllAsync();
    Task<IEnumerable<NoteDto>> GetAllAsync(Guid userId);
}