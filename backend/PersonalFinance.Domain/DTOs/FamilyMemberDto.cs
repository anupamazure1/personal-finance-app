namespace PersonalFinance.Domain.DTOs;

public class FamilyMemberDto
{
    public int Id { get; set; }
    public int FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public decimal NetWorth { get; set; }
    public List<BankAccountDto> Accounts { get; set; } = new();
}
