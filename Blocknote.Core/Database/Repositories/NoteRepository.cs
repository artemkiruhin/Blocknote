using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocknote.Core.Database.Repositories;

public class NoteRepository : BaseCrudRepository<NoteEntity>, INoteRepository
{
    public NoteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<NoteEntity>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
    }
}