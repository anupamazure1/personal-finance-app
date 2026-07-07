namespace PersonalFinance.Api.Services;

using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain.DTOs;
using PersonalFinance.Domain.Entities;

public interface IFamilyService
{
    Task<FamilyDto?> GetFamilyByIdAsync(int id);
    Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync();
    Task<FamilyDto> CreateFamilyAsync(string name, string? description);
    Task<FamilyDto> UpdateFamilyAsync(int id, string name, string? description);
    Task DeleteFamilyAsync(int id);
}

public class FamilyService : IFamilyService
{
    private readonly IFamilyRepository _repository;

    public FamilyService(IFamilyRepository repository)
    {
        _repository = repository;
    }

    public async Task<FamilyDto?> GetFamilyByIdAsync(int id)
    {
        var family = await _repository.GetByIdAsync(id);
        return family != null ? MapToDto(family) : null;
    }

    public async Task<IEnumerable<FamilyDto>> GetAllFamiliesAsync()
    {
        var families = await _repository.GetAllAsync();
        return families.Select(MapToDto);
    }

    public async Task<FamilyDto> CreateFamilyAsync(string name, string? description)
    {
        var family = new Family
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(family);
        return MapToDto(family);
    }

    public async Task<FamilyDto> UpdateFamilyAsync(int id, string name, string? description)
    {
        var family = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Family with ID {id} not found");

        family.Name = name;
        family.Description = description;
        family.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(family);
        return MapToDto(family);
    }

    public async Task DeleteFamilyAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private FamilyDto MapToDto(Family family)
    {
        return new FamilyDto
        {
            Id = family.Id,
            Name = family.Name,
            Description = family.Description,
            Members = family.Members?.Select(m => new FamilyMemberDto
            {
                Id = m.Id,
                FamilyId = m.FamilyId,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Relationship = m.Relationship,
                Accounts = m.Accounts?.Select(a => new BankAccountDto
                {
                    Id = a.Id,
                    MemberId = a.MemberId,
                    BankName = a.BankName,
                    AccountType = a.AccountType,
                    AccountNumber = a.AccountNumber,
                    AccountHolder = a.AccountHolder,
                    CurrentBalance = a.CurrentBalance
                }).ToList() ?? new()
            }).ToList() ?? new()
        };
    }
}
