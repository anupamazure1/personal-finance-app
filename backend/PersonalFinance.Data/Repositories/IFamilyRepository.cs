using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Data.Repositories;

public interface IFamilyRepository
{
    Task<Family?> GetByIdAsync(int id);
    Task<IEnumerable<Family>> GetAllAsync();
    Task AddAsync(Family family);
    Task UpdateAsync(Family family);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
