using SocialX.Core.Interface;
using SocialX.Infra.Data;

namespace SocialX.Infra.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomDbContext _context;

    public UnitOfWork(CustomDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SalvarAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
