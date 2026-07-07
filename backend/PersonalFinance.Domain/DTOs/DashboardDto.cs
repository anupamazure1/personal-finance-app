namespace PersonalFinance.Domain.DTOs;

public class DashboardSummaryDto
{
    public decimal TotalFamilyNetWorth { get; set; }
    public int TotalFamilies { get; set; }
    public int TotalMembers { get; set; }
    public int TotalAccounts { get; set; }
    public List<FamilyNetWorthDto> FamiliesNetWorth { get; set; } = new();
}

public class FamilyNetWorthDto
{
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public decimal NetWorth { get; set; }
    public List<MemberNetWorthDto> Members { get; set; } = new();
}

public class MemberNetWorthDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public decimal NetWorth { get; set; }
    public List<AccountNetWorthDto> Accounts { get; set; } = new();
}

public class AccountNetWorthDto
{
    public int AccountId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
