using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Core.Parsers;

public class KotakBankParser : IBankStatementParser
{
    public bool SupportsFormat(string fileName)
    {
        return fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<BankTransaction>> ParseAsync(Stream fileStream, string fileName)
    {
        var transactions = new List<BankTransaction>();

        try
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<KotakTransactionMap>();
                var records = csv.GetRecords<KotakTransactionRecord>();

                foreach (var record in records)
                {
                    if (string.IsNullOrWhiteSpace(record.Particulars))
                        continue;

                    var transaction = new KotakBankTransaction
                    {
                        TransactionDate = DateTime.ParseExact(record.TransactionDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                        Description = record.Particulars,
                        Amount = ParseAmount(record.Amount),
                        ChequeNumber = record.ChequeNumber,
                        Balance = ParseAmount(record.Balance),
                        TransactionType = record.Amount.Contains("-") ? "DR" : "CR"
                    };

                    transactions.Add(transaction);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error parsing Kotak bank statement: {ex.Message}", ex);
        }

        return transactions;
    }

    private decimal ParseAmount(string amount)
    {
        if (string.IsNullOrWhiteSpace(amount))
            return 0;

        var cleaned = amount.Replace(" ", "").Replace(",", "");
        if (decimal.TryParse(cleaned, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
            return result;

        return 0;
    }
}

public class KotakTransactionRecord
{
    public string TransactionDate { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string ChequeNumber { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
}

public sealed class KotakTransactionMap : ClassMap<KotakTransactionRecord>
{
    public KotakTransactionMap()
    {
        Map(m => m.TransactionDate).Name("Txn Date");
        Map(m => m.Particulars).Name("Particulars");
        Map(m => m.ChequeNumber).Name("Cheque #");
        Map(m => m.Amount).Name("Amount");
        Map(m => m.Balance).Name("Balance");
    }
}
