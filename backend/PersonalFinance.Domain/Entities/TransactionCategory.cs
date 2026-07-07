namespace PersonalFinance.Domain.Entities;

/// <summary>
/// Transaction category for classification
/// </summary>
public class TransactionCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconColor { get; set; }
}
