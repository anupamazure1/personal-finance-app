using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(int id);
    Task<IEnumerable<BankAccount>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<BankAccount>> GetAllAsync();
    Task AddAsync(BankAccount account);
    Task UpdateAsync(BankAccount account);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
