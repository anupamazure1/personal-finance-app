using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data.Context;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // HDFC Methods
    public async Task<HdfcBankTransaction?> GetHdfcTransactionAsync(long id)
    {
        return await _context.HdfcBankTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<HdfcBankTransaction>> GetHdfcTransactionsByAccountAsync(int accountId)
    {
        return await _context.HdfcBankTransactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddHdfcTransactionsAsync(IEnumerable<HdfcBankTransaction> transactions)
    {
        await _context.HdfcBankTransactions.AddRangeAsync(transactions);
        await SaveChangesAsync();
    }

    // ICICI Methods
    public async Task<IciciBankTransaction?> GetIciciTransactionAsync(long id)
    {
        return await _context.IciciBankTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<IciciBankTransaction>> GetIciciTransactionsByAccountAsync(int accountId)
    {
        return await _context.IciciBankTransactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddIciciTransactionsAsync(IEnumerable<IciciBankTransaction> transactions)
    {
        await _context.IciciBankTransactions.AddRangeAsync(transactions);
        await SaveChangesAsync();
    }

    // Kotak Methods
    public async Task<KotakBankTransaction?> GetKotakTransactionAsync(long id)
    {
        return await _context.KotakBankTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<KotakBankTransaction>> GetKotakTransactionsByAccountAsync(int accountId)
    {
        return await _context.KotakBankTransactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddKotakTransactionsAsync(IEnumerable<KotakBankTransaction> transactions)
    {
        await _context.KotakBankTransactions.AddRangeAsync(transactions);
        await SaveChangesAsync();
    }

    // Yes Bank Methods
    public async Task<YesBankTransaction?> GetYesBankTransactionAsync(long id)
    {
        return await _context.YesBankTransactions.FindAsync(id);
    }

    public async Task<IEnumerable<YesBankTransaction>> GetYesBankTransactionsByAccountAsync(int accountId)
    {
        return await _context.YesBankTransactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddYesBankTransactionsAsync(IEnumerable<YesBankTransaction> transactions)
    {
        await _context.YesBankTransactions.AddRangeAsync(transactions);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
