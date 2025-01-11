using Blocknote.Core.Models.Entities;

namespace Blocknote.Core.Database.Repositories.Base;

public interface INoteRepository : ICrudRepository<NoteEntity>
{
    Task<IEnumerable<NoteEntity>> GetByUserIdAsync(Guid userId);
}