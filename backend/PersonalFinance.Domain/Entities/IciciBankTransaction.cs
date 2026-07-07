namespace PersonalFinance.Domain.Entities;

/// <summary>
/// ICICI Bank transaction
/// </summary>
public class IciciBankTransaction : BankTransaction
{
    public string? TransactionType { get; set; } // DR, CR
}
