namespace Blocknote.Core.Services.Sharing;

public interface ISharingService
{
    Task<string> GenerateSharingCode(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.All);
    Task<string> GetSharingCodeAsync(string code, Guid? userId);
    Task<bool> DeleteSharingCodeAsync(string code, Guid userId);
    Task<string> CreateSharingCodeAsync(Guid userId, Guid noteId,  DateTime closeTime, SharingType type = SharingType.All);
}