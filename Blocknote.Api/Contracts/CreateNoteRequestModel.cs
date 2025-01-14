namespace Blocknote.Api.Contracts;

public record CreateNoteRequestModel(string Title, string? Subtitle, string? Content);