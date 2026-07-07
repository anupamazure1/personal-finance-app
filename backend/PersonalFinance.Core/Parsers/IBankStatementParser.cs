using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Core.Parsers;

public interface IBankStatementParser
{
    Task<List<BankTransaction>> ParseAsync(Stream fileStream, string fileName);
    bool SupportsFormat(string fileName);
}
