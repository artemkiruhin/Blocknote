using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Dtos;
using Blocknote.Core.Models.Entities;
using Blocknote.Core.Models.Enums;
using Blocknote.Core.Services.Base;
using Blocknote.Core.Services.Hasher;

namespace Blocknote.Core.Services.Entity;

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
            NoteId = sharingNote.NoteId,
            Code = sharingNote.Code
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
            AuthorUsername = author.Username,
            Code = sharingNote.Code
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
            AuthorUsername = author.Username,
            Code = sharingNote.Code,
            AccessType = sharingNote.Type == nameof(SharingType.All) ? "public" : "registered"
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
            NoteId = x.NoteId,
            AuthorUsername = x.User.Username,
            CloseAt = x.CloseAt,
            Content = x.Note.Content,
            Subtitle = x.Note.Subtitle,
            Type = x.Type,
            Title = x.Note.Title,
            Code = x.Code,
            AccessType = x.Type == nameof(SharingType.All) ? "public" : "registered"
        });
    }

    public async Task<bool> DeleteSharingCodeAsync(string code, Guid userId)
    {
        var sharingNote = await _sharingRepository.GetByIdAsync(Guid.Parse(code)) ?? throw new KeyNotFoundException();
        if (sharingNote.UserId != userId) throw new UnauthorizedAccessException();
        var result = await _sharingRepository.DeleteAsync(sharingNote.Id);
        return result;
    }

    public async Task<SharingCreateResponse> CreateSharingAsync(Guid userId, Guid noteId, DateTime closeTime, SharingType type = SharingType.All)
    {
        try
        {
            var openTime = DateTime.UtcNow;
            if (closeTime < openTime) throw new ArgumentException("Close time must be greater than open time.");

            var noteEntity = await _noteRepository.GetByIdAsync(noteId) ?? throw new KeyNotFoundException();
            if (noteEntity.UserId != userId) throw new UnauthorizedAccessException();

            var id = Guid.NewGuid();
            var code = $"share_{id}";

            var sharingNote = SharingNoteEntity.Create(id, noteId, code, userId, closeTime, type);
            var result = await _sharingRepository.AddAsync(sharingNote);

            var noteResult = new SharingCreateResponse(id, code);

            if (!result) throw new ApplicationException("Something went wrong");

            return noteResult;
        }
        catch
        {
            throw;
        }
    }

    public async Task<SharingNoteDto> UpdateSharingAsync(Guid id, bool isAllowedAll, bool hasExpires, DateTime? expiresAt)
    {
        var sharing = await _sharingRepository.GetByIdAsync(id);
        if (sharing == null) throw new KeyNotFoundException();

        sharing.Type = isAllowedAll ? nameof(SharingType.All) : nameof(SharingType.Registered);

        if (hasExpires)
        {
            if (!expiresAt.HasValue)
            {
                throw new ArgumentException("ExpiresAt must have a value when hasExpires is true.");
            }

            if (expiresAt.Value <= DateTime.UtcNow)
            {
                throw new ArgumentException("ExpiresAt must be greater than the current time.");
            }

            sharing.CloseAt = expiresAt.Value;
        }

        var result = await _sharingRepository.EditAsync(sharing);
        var note = await _noteRepository.GetByIdAsync(sharing.NoteId);
        var author = await _userRepository.GetByIdAsync(sharing.UserId);

        var resultDto = new SharingNoteDto
        {
            Id = sharing.Id,
            NoteId = sharing.NoteId,
            UserId = sharing.UserId,
            Code = sharing.Code,
            Type = sharing.Type,
            CreatedAt = sharing.CreatedAt,
            CloseAt = sharing.CloseAt,
            Title = note.Title,
            Subtitle = note.Subtitle,
            Content = note.Content,
            AuthorUsername = author.Username,
            AccessType = sharing.Type == nameof(SharingType.All) ? "public" : "registered"
        };

        return resultDto;
    }
}