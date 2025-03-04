using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blocknote.Core.Database.Repositories;

public class SharingNoteRepository : BaseCrudRepository<SharingNoteEntity>, ISharingRepository
{
    public SharingNoteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SharingNoteEntity>> GetByUserAsync(Guid userId)
    {
        return await _dbSet.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
    }
    
    public async Task<SharingNoteEntity?> GetByCodeAsync(string code)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
    }
}