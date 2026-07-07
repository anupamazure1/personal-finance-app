namespace PersonalFinance.Domain.DTOs;

public class BankAccountDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountHolder { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal? OpeningBalance { get; set; }
    public DateTime? OpeningDate { get; set; }
}
