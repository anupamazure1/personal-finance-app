using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data.Context;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly ApplicationDbContext _context;

    public BankAccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccount?> GetByIdAsync(int id)
    {
        return await _context.BankAccounts.FindAsync(id);
    }

    public async Task<IEnumerable<BankAccount>> GetByMemberIdAsync(int memberId)
    {
        return await _context.BankAccounts
            .Where(a => a.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<IEnumerable<BankAccount>> GetAllAsync()
    {
        return await _context.BankAccounts.ToListAsync();
    }

    public async Task AddAsync(BankAccount account)
    {
        await _context.BankAccounts.AddAsync(account);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(BankAccount account)
    {
        account.UpdatedAt = DateTime.UtcNow;
        _context.BankAccounts.Update(account);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _context.BankAccounts.FindAsync(id);
        if (account != null)
        {
            _context.BankAccounts.Remove(account);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
