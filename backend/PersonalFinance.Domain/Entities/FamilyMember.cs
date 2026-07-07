namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Represents a family member
/// </summary>
public class FamilyMember
{
    public int Id { get; set; }
    public int FamilyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Family? Family { get; set; }
    public virtual ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();
}
