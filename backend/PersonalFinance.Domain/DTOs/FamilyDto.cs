namespace PersonalFinance.Domain.DTOs;

public class FamilyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<FamilyMemberDto> Members { get; set; } = new();
}
