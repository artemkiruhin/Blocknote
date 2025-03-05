namespace Blocknote.Api.Contracts;

public record SharingCreateRequestModel(Guid NoteId, DateTime? FinishDate, bool AllowedAll, bool HasFinishDate);