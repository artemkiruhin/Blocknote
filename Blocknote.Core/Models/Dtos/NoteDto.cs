﻿namespace Blocknote.Core.Models.Dtos;

public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}