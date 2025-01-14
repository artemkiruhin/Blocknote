using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Entities;
using Blocknote.Core.Services.Base;

namespace Blocknote.Core.Services.Entity;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;

    public NoteService(INoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
    {
        var notes = await _repository.GetAllAsync();
        return notes.Select(x => new NoteDto()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Subtitle = x.Subtitle
        });
    }
    
    public async Task<IEnumerable<NoteDto>> GetAllAsync(Guid userId)
    {
        var notes = await _repository.GetByUserIdAsync(userId);
        return notes.Select(x => new NoteDto()
        {
            Id = x.Id,
            Title = x.Title,
            Content = x.Content,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Subtitle = x.Subtitle
        });
    }

    public async Task<NoteDto?> GetInfoAsync(Guid userId, Guid noteId)
    {
        try
        {
            var note = await _repository.GetByIdAsync(noteId);
            if (note?.UserId != userId) return null;
            return new NoteDto()
            {
                Id = note.Id,
                Title = note.Title,
                Subtitle = note.Subtitle,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }
        catch (Exception e)
        {

            return null;
        }
    }

    public async Task<IEnumerable<NoteDto>> GetByFullContentAsync(string fullContent)
    {
        try
        {
            var notes = await _repository.FindAsync(x =>
                string.Concat(x.Content, x.Title, x.Subtitle).Contains(fullContent));
            var dtos = notes.Select(x => new NoteDto()
            {
                Id = x.Id,
                Title = x.Title,
                Subtitle = x.Subtitle,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
            return dtos;
        }
        catch (Exception e)
        {
            return [];
        }
    }

    public async Task<bool> CreateAsync(string title, string? subtitle, string? content, Guid userId)
    {
        try
        {
            var note = NoteEntity.Create(title, subtitle, content, userId);
            return await _repository.AddAsync(note);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> EditAsync(Guid userId, Guid noteId, string title, string? subtitle, string? content)
    {
        try
        {
            var note = await _repository.GetByIdAsync(noteId);
            if (note == null) return false;
            
            if (note.UserId != userId) return false;
            
            if (subtitle != null) note.Subtitle = subtitle;
            if (content != null) note.Content = content;
            note.UpdatedAt = DateTime.UtcNow;
            
            return await _repository.EditAsync(note);
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUser(Guid userId, Guid noteId)
    {
        try
        {
            var note = await _repository.GetByIdAsync(noteId);
            if (note == null) return false;
            if (note.UserId != userId) return false;
            
            return await _repository.DeleteAsync(noteId);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}