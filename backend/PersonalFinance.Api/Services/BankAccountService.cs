namespace PersonalFinance.Api.Services;

using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain.DTOs;
using PersonalFinance.Domain.Entities;

public interface IBankAccountService
{
    Task<BankAccountDto?> GetAccountByIdAsync(int id);
    Task<IEnumerable<BankAccountDto>> GetAccountsByMemberAsync(int memberId);
    Task<IEnumerable<BankAccountDto>> GetAllAccountsAsync();
    Task<BankAccountDto> CreateAccountAsync(int memberId, string bankName, string accountType, string accountNumber, string? accountHolder, decimal balance);
    Task<BankAccountDto> UpdateAccountAsync(int id, decimal balance);
    Task DeleteAccountAsync(int id);
}

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _repository;

    public BankAccountService(IBankAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<BankAccountDto?> GetAccountByIdAsync(int id)
    {
        var account = await _repository.GetByIdAsync(id);
        return account != null ? MapToDto(account) : null;
    }

    public async Task<IEnumerable<BankAccountDto>> GetAccountsByMemberAsync(int memberId)
    {
        var accounts = await _repository.GetByMemberIdAsync(memberId);
        return accounts.Select(MapToDto);
    }

    public async Task<IEnumerable<BankAccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _repository.GetAllAsync();
        return accounts.Select(MapToDto);
    }

    public async Task<BankAccountDto> CreateAccountAsync(int memberId, string bankName, string accountType, string accountNumber, string? accountHolder, decimal balance)
    {
        var account = new BankAccount
        {
            MemberId = memberId,
            BankName = bankName,
            AccountType = accountType,
            AccountNumber = accountNumber,
            AccountHolder = accountHolder,
            CurrentBalance = balance,
            OpeningBalance = balance,
            OpeningDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(account);
        return MapToDto(account);
    }

    public async Task<BankAccountDto> UpdateAccountAsync(int id, decimal balance)
    {
        var account = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Account with ID {id} not found");

        account.CurrentBalance = balance;
        account.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(account);
        return MapToDto(account);
    }

    public async Task DeleteAccountAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private BankAccountDto MapToDto(BankAccount account)
    {
        return new BankAccountDto
        {
            Id = account.Id,
            MemberId = account.MemberId,
            BankName = account.BankName,
            AccountType = account.AccountType,
            AccountNumber = account.AccountNumber,
            AccountHolder = account.AccountHolder,
            CurrentBalance = account.CurrentBalance,
            OpeningBalance = account.OpeningBalance,
            OpeningDate = account.OpeningDate
        };
    }
}
