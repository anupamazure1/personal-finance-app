namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Represents a bank account
/// </summary>
public class BankAccount
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty; // Savings, Current, FD, MutualFund, Trading, Demat
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountHolder { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal? OpeningBalance { get; set; }
    public DateTime? OpeningDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual FamilyMember? Member { get; set; }
}
