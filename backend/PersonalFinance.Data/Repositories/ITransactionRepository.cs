using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public interface ITransactionRepository
{
    Task<HdfcBankTransaction?> GetHdfcTransactionAsync(long id);
    Task<IEnumerable<HdfcBankTransaction>> GetHdfcTransactionsByAccountAsync(int accountId);
    Task AddHdfcTransactionsAsync(IEnumerable<HdfcBankTransaction> transactions);

    Task<IciciBankTransaction?> GetIciciTransactionAsync(long id);
    Task<IEnumerable<IciciBankTransaction>> GetIciciTransactionsByAccountAsync(int accountId);
    Task AddIciciTransactionsAsync(IEnumerable<IciciBankTransaction> transactions);

    Task<KotakBankTransaction?> GetKotakTransactionAsync(long id);
    Task<IEnumerable<KotakBankTransaction>> GetKotakTransactionsByAccountAsync(int accountId);
    Task AddKotakTransactionsAsync(IEnumerable<KotakBankTransaction> transactions);

    Task<YesBankTransaction?> GetYesBankTransactionAsync(long id);
    Task<IEnumerable<YesBankTransaction>> GetYesBankTransactionsByAccountAsync(int accountId);
    Task AddYesBankTransactionsAsync(IEnumerable<YesBankTransaction> transactions);

    Task SaveChangesAsync();
}
