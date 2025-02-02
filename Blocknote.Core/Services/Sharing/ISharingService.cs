using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Enums;

namespace Blocknote.Core.Services.Sharing;

public interface ISharingService
{
    Task<SharingNoteDto> GetSharingCodeAsync(string code, Guid? userId);
    Task<SharingNoteDto> GetSharingCodeAsync(Guid code);
    Task<SharingNoteDto> GetSharingCodeAsync(string code);
    Task<IEnumerable<SharingNoteDto>> GetSharingsAsync(Guid userId);
    Task<bool> DeleteSharingCodeAsync(string code, Guid userId);
    Task<string> CreateSharingCodeAsync(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.All);
    string FormatContent(string content, FormatType type = FormatType.HTML);
}