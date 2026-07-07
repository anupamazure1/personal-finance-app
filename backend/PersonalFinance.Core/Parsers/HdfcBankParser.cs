using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Core.Parsers;

public class HdfcBankParser : IBankStatementParser
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
                csv.Context.RegisterClassMap<HdfcTransactionMap>();
                var records = csv.GetRecords<HdfcTransactionRecord>();

                foreach (var record in records)
                {
                    if (string.IsNullOrWhiteSpace(record.Description))
                        continue;

                    var transaction = new HdfcBankTransaction
                    {
                        TransactionDate = DateTime.ParseExact(record.TransactionDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                        ValueDate = TryParseDate(record.ValueDate, "dd-MMM-yyyy"),
                        Description = record.Description,
                        Amount = ParseAmount(record.Amount),
                        Balance = ParseAmount(record.Balance),
                        ReferenceNumber = record.ReferenceNumber,
                        ChequeNumber = record.ChequeNumber,
                        TransactionType = record.Amount.Contains("-") ? "DR" : "CR"
                    };

                    transactions.Add(transaction);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error parsing HDFC bank statement: {ex.Message}", ex);
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

    private DateTime? TryParseDate(string? dateString, string format)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return null;

        if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            return result;

        return null;
    }
}

public class HdfcTransactionRecord
{
    public string TransactionDate { get; set; } = string.Empty;
    public string ValueDate { get; set; } = string.Empty;
    public string ChequeNumber { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
}

public sealed class HdfcTransactionMap : ClassMap<HdfcTransactionRecord>
{
    public HdfcTransactionMap()
    {
        Map(m => m.TransactionDate).Name("Transaction Date");
        Map(m => m.ValueDate).Name("Value Date");
        Map(m => m.ChequeNumber).Name("Cheque #");
        Map(m => m.ReferenceNumber).Name("Reference #");
        Map(m => m.Description).Name("Description");
        Map(m => m.Amount).Name("Amount");
        Map(m => m.Balance).Name("Balance");
    }
}
