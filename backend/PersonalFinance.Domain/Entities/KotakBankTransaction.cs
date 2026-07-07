namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Kotak Bank transaction
/// </summary>
public class KotakBankTransaction : BankTransaction
{
    public string? TransactionType { get; set; } // DR, CR
}
