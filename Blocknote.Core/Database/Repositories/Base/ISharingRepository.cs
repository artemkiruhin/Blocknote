using Blocknote.Core.Models.Entities;

namespace Blocknote.Core.Database.Repositories.Base;

public interface ISharingRepository : ICrudRepository<SharingNoteEntity>
{
    Task<IEnumerable<SharingNoteEntity>> GetByUserAsync(Guid userId);
    Task<SharingNoteEntity?> GetByCodeAsync(string code);
}