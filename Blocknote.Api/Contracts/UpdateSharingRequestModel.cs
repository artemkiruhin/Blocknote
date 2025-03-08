namespace Blocknote.Api.Contracts;

public record UpdateSharingRequestModel(Guid Id, bool IsAllowedAll, bool HasExpires, DateTime? ExpiresAt);