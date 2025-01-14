namespace Blocknote.Api.Contracts;

public record UpdateNoteRequestModel(Guid Id, string Title, string? Subtitle, string? Content);