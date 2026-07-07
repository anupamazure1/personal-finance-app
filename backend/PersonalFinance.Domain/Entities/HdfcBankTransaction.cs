namespace PersonalFinance.Domain.Entities;

/// <summary>
/// HDFC Bank transaction
/// </summary>
public class HdfcBankTransaction : BankTransaction
{
    public string? TransactionType { get; set; } // DR, CR
}
