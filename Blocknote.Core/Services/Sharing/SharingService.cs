using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Entities;
using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Hasher;

namespace Blocknote.Core.Services.Sharing;

public class SharingService : ISharingService
{
    private readonly IUserRepository _userRepository;
    private readonly INoteRepository _noteRepository;
    private readonly ISharingRepository _sharingRepository;
    private readonly IHashService _hashService;

    public SharingService(IUserRepository userRepository, INoteRepository noteRepository, ISharingRepository sharingRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _noteRepository = noteRepository;
        _sharingRepository = sharingRepository;
        _hashService = hashService;
    }
    
    public async Task<SharingNoteDto> GetSharingCodeAsync(string code, Guid? userId)
    {
        var sharingNote = await _sharingRepository.GetByIdAsync(Guid.Parse(code)) ?? throw new KeyNotFoundException();
        if (DateTime.UtcNow > sharingNote.CloseAt)
        {
            await _sharingRepository.DeleteAsync(sharingNote.Id);
            throw new KeyNotFoundException();
        }
        if (sharingNote.Type == nameof(SharingType.Registered) && !userId.HasValue) throw new UnauthorizedAccessException();

        return new SharingNoteDto()
        {
            Id = sharingNote.Id,
            UserId = sharingNote.UserId,
            CreatedAt = sharingNote.CreatedAt,
            NoteId = sharingNote.NoteId
        };
    }

    public async Task<SharingNoteDto> GetSharingCodeAsync(Guid code)
    {
        var sharingNote = await _sharingRepository.GetByIdAsync(code) ?? throw new KeyNotFoundException();

        if (DateTime.UtcNow > sharingNote.CloseAt)
        {
            await _sharingRepository.DeleteAsync(sharingNote.Id);
            throw new KeyNotFoundException();
        }

        if (sharingNote.Type == nameof(SharingType.Registered))
            throw new UnauthorizedAccessException();

        // Получаем связанную заметку
        var note = await _noteRepository.GetByIdAsync(sharingNote.NoteId);
        // Получаем автора
        var author = await _userRepository.GetByIdAsync(sharingNote.UserId);

        return new SharingNoteDto()
        {
            Id = sharingNote.Id,
            UserId = sharingNote.UserId,
            NoteId = sharingNote.NoteId,
            CreatedAt = sharingNote.CreatedAt,
            CloseAt = sharingNote.CloseAt,
            Type = sharingNote.Type,
            // Добавляем данные из связанной заметки
            Title = note.Title,
            Subtitle = note.Subtitle,
            Content = note.Content,
            // Добавляем имя автора
            AuthorUsername = author.Username
        };
    }

    public async Task<SharingNoteDto> GetSharingCodeAsync(string code)
    {
        var sharingNote = await _sharingRepository.GetByIdAsync(Guid.Parse(code)) ?? throw new KeyNotFoundException();

        if (DateTime.UtcNow > sharingNote.CloseAt)
        {
            await _sharingRepository.DeleteAsync(sharingNote.Id);
            throw new KeyNotFoundException();
        }

        if (sharingNote.Type == nameof(SharingType.Registered))
            throw new UnauthorizedAccessException();

        // Получаем связанную заметку
        var note = await _noteRepository.GetByIdAsync(sharingNote.NoteId);
        // Получаем автора
        var author = await _userRepository.GetByIdAsync(sharingNote.UserId);

        return new SharingNoteDto()
        {
            Id = sharingNote.Id,
            UserId = sharingNote.UserId,
            NoteId = sharingNote.NoteId,
            CreatedAt = sharingNote.CreatedAt,
            CloseAt = sharingNote.CloseAt,
            Type = sharingNote.Type,
            // Добавляем данные из связанной заметки
            Title = note.Title,
            Subtitle = note.Subtitle,
            Content = note.Content,
            // Добавляем имя автора
            AuthorUsername = author.Username
        };
    }

    public async Task<IEnumerable<SharingNoteDto>> GetSharingsAsync(Guid userId)
    {
        var entities = await _sharingRepository.GetByUserAsync(userId);
        return entities.Select(x => new SharingNoteDto()
        {
            Id = x.Id,
            UserId = x.UserId,
            CreatedAt = x.CreatedAt,
            NoteId = x.NoteId
        });
    }

    public async Task<bool> DeleteSharingCodeAsync(string code, Guid userId)
    {
        var sharingNote = await _sharingRepository.GetByIdAsync(Guid.Parse(code)) ?? throw new KeyNotFoundException();
        if (sharingNote.UserId != userId) throw new UnauthorizedAccessException();
        var result = await _sharingRepository.DeleteAsync(sharingNote.Id);
        return result;
    }

    public async Task<string> CreateSharingCodeAsync(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.All)
    {
        var openTime = DateTime.UtcNow;
        if (closeTime < openTime) throw new ArgumentException("Close time must be greater than open time.");
        
        var noteEntity = await _noteRepository.GetByIdAsync(noteId) ?? throw new KeyNotFoundException();
        if (noteEntity.UserId != userId) throw new UnauthorizedAccessException();
        
        var sharingNote = SharingNoteEntity.Create(noteId, userId, closeTime, type);
        var result = await _sharingRepository.AddAsync(sharingNote);
        
        return result ? sharingNote.Id.ToString() : string.Empty;
    }
}