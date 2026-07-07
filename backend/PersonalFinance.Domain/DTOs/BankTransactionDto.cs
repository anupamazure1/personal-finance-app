namespace PersonalFinance.Domain.DTOs;

public class BankTransactionDto
{
    public long Id { get; set; }
    public int AccountId { get; set; }
    public DateTime TransactionDate { get; set; }
    public DateTime? ValueDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? Balance { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? ChequeNumber { get; set; }
    public string? Category { get; set; }
    public string Bank { get; set; } = string.Empty;
}
