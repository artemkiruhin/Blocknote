﻿namespace Blocknote.Core.Services.Sharing;

public interface ISharingService
{
    string GenerateSharingCode(Guid userId, Guid noteId,  DateTime closeTime);
    Task<string> GetSharingCodeAsync(string code, Guid? userId);
    Task<bool> DeleteSharingCodeAsync(string code, Guid userId);
    Task<string> CreateSharingCodeAsync(Guid userId, Guid noteId,  DateTime closeTime);
}