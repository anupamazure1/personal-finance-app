namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Represents a family entity
/// </summary>
public class Family
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<FamilyMember> Members { get; set; } = new List<FamilyMember>();
}
