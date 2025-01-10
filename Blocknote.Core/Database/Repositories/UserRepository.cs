using Blocknote.Core.Database.Repositories.Base;
using Blocknote.Core.Models.Entities;

namespace Blocknote.Core.Database.Repositories;

public class UserRepository : BaseCrudRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}