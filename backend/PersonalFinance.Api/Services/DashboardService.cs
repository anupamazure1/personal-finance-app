namespace PersonalFinance.Api.Services;

using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain.DTOs;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<decimal> GetFamilyNetWorthAsync(int familyId);
    Task<decimal> GetMemberNetWorthAsync(int memberId);
}

public class DashboardService : IDashboardService
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IBankAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DashboardService(
        IFamilyRepository familyRepository,
        IBankAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _familyRepository = familyRepository;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var families = await _familyRepository.GetAllAsync();
        var accounts = await _accountRepository.GetAllAsync();

        var familiesNetWorth = new List<FamilyNetWorthDto>();
        decimal totalNetWorth = 0;

        foreach (var family in families)
        {
            var familyNetWorth = new FamilyNetWorthDto
            {
                FamilyId = family.Id,
                FamilyName = family.Name,
                Members = new()
            };

            foreach (var member in family.Members)
            {
                var memberAccounts = accounts.Where(a => a.MemberId == member.Id).ToList();
                var memberNetWorth = memberAccounts.Sum(a => a.CurrentBalance);

                familyNetWorth.Members.Add(new MemberNetWorthDto
                {
                    MemberId = member.Id,
                    MemberName = member.Name,
                    NetWorth = memberNetWorth,
                    Accounts = memberAccounts.Select(a => new AccountNetWorthDto
                    {
                        AccountId = a.Id,
                        BankName = a.BankName,
                        AccountType = a.AccountType,
                        Balance = a.CurrentBalance
                    }).ToList()
                });

                familyNetWorth.NetWorth += memberNetWorth;
            }

            familiesNetWorth.Add(familyNetWorth);
            totalNetWorth += familyNetWorth.NetWorth;
        }

        return new DashboardSummaryDto
        {
            TotalFamilyNetWorth = totalNetWorth,
            TotalFamilies = families.Count(),
            TotalMembers = families.SelectMany(f => f.Members).Count(),
            TotalAccounts = accounts.Count(),
            FamiliesNetWorth = familiesNetWorth
        };
    }

    public async Task<decimal> GetFamilyNetWorthAsync(int familyId)
    {
        var family = await _familyRepository.GetByIdAsync(familyId);
        if (family == null)
            throw new KeyNotFoundException($"Family with ID {familyId} not found");

        var accounts = await _accountRepository.GetAllAsync();
        var familyAccounts = accounts.Where(a => family.Members.Any(m => m.Id == a.MemberId));

        return familyAccounts.Sum(a => a.CurrentBalance);
    }

    public async Task<decimal> GetMemberNetWorthAsync(int memberId)
    {
        var accounts = await _accountRepository.GetByMemberIdAsync(memberId);
        return accounts.Sum(a => a.CurrentBalance);
    }
}
