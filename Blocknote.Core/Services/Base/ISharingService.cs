using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Enums;

namespace Blocknote.Core.Services.Base;

public interface ISharingService
{
    Task<SharingNoteDto> GetSharingCodeAsync(string code, Guid? userId);
    Task<SharingNoteDto> GetSharingCodeAsync(Guid code);
    Task<SharingNoteDto> GetSharingCodeAsync(string code);
    Task<IEnumerable<SharingNoteDto>> GetSharingsAsync(Guid userId);
    Task<bool> DeleteSharingCodeAsync(string code, Guid userId);
    Task<SharingCreateResponse> CreateSharingAsync(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.Public);
    Task<SharingNoteDto> UpdateSharingAsync(Guid id, bool isAllowedAll, bool hasExpires, DateTime? expiresAt);
}