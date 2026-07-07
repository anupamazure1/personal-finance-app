using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Core.Parsers;

public class BankParserFactory
{
    private readonly Dictionary<string, IBankStatementParser> _parsers;

    public BankParserFactory()
    {
        _parsers = new Dictionary<string, IBankStatementParser>
        {
            { "hdfc", new HdfcBankParser() },
            { "icici", new IciciBankParser() },
            { "kotak", new KotakBankParser() },
            { "yesbank", new YesBankParser() }
        };
    }

    public IBankStatementParser? GetParser(string bankName)
    {
        return _parsers.TryGetValue(bankName.ToLower(), out var parser) ? parser : null;
    }
}
