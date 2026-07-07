using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data.Context;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public class FamilyRepository : IFamilyRepository
{
    private readonly ApplicationDbContext _context;

    public FamilyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Family?> GetByIdAsync(int id)
    {
        return await _context.Families
            .Include(f => f.Members)
                .ThenInclude(m => m.Accounts)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Family>> GetAllAsync()
    {
        return await _context.Families
            .Include(f => f.Members)
                .ThenInclude(m => m.Accounts)
            .ToListAsync();
    }

    public async Task AddAsync(Family family)
    {
        await _context.Families.AddAsync(family);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Family family)
    {
        family.UpdatedAt = DateTime.UtcNow;
        _context.Families.Update(family);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var family = await _context.Families.FindAsync(id);
        if (family != null)
        {
            _context.Families.Remove(family);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
