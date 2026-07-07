namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Yes Bank transaction
/// </summary>
public class YesBankTransaction : BankTransaction
{
    public string? TransactionType { get; set; } // DR, CR
}
