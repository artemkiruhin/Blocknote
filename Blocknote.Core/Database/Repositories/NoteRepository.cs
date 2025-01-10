using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Entities;

namespace Blocknote.Core.Database.Repositories;

public class NoteRepository : BaseCrudRepository<NoteEntity>, INoteRepository
{
    public NoteRepository(AppDbContext context) : base(context)
    {
    }
}