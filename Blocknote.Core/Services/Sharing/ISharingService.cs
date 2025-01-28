using Blocknote.Core.Models.Dtos;

namespace Blocknote.Core.Services.Sharing;

public interface ISharingService
{
    Task<SharingNoteDto> GetSharingCodeAsync(string code, Guid? userId);
    Task<IEnumerable<SharingNoteDto>> GetSharingsAsync(Guid userId);
    Task<bool> DeleteSharingCodeAsync(string code, Guid userId);
    Task<string> CreateSharingCodeAsync(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.All);
}