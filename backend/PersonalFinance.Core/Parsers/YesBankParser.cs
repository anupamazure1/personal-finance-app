using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PersonalFinance.Domain.Entities;

namespace PersonalFinance.Core.Parsers;

public class YesBankParser : IBankStatementParser
{
    public bool SupportsFormat(string fileName)
    {
        return fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) || 
               fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<List<BankTransaction>> ParseAsync(Stream fileStream, string fileName)
    {
        var transactions = new List<BankTransaction>();

        try
        {
            if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                transactions = ParseExcel(fileStream);
            }
            else if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                transactions = await ParseCsv(fileStream);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error parsing Yes Bank statement: {ex.Message}", ex);
        }

        return transactions;
    }

    private List<BankTransaction> ParseExcel(Stream fileStream)
    {
        var transactions = new List<BankTransaction>();
        var workbook = new XSSFWorkbook(fileStream);
        var sheet = workbook.GetSheetAt(0);

        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            var row = sheet.GetRow(i);
            if (row == null)
                continue;

            try
            {
                var transaction = new YesBankTransaction
                {
                    TransactionDate = GetCellDate(row.GetCell(0)),
                    Description = GetCellString(row.GetCell(1)),
                    Amount = GetCellDecimal(row.GetCell(2)),
                    ChequeNumber = GetCellString(row.GetCell(3)),
                    Balance = GetCellDecimal(row.GetCell(4)),
                    TransactionType = GetCellDecimal(row.GetCell(2)) >= 0 ? "CR" : "DR"
                };

                if (!string.IsNullOrWhiteSpace(transaction.Description))
                    transactions.Add(transaction);
            }
            catch
            {
                continue;
            }
        }

        return transactions;
    }

    private async Task<List<BankTransaction>> ParseCsv(Stream fileStream)
    {
        var transactions = new List<BankTransaction>();

        using (var reader = new StreamReader(fileStream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<YesBankTransactionMap>();
            var records = csv.GetRecords<YesBankTransactionRecord>();

            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.Description))
                    continue;

                var transaction = new YesBankTransaction
                {
                    TransactionDate = DateTime.ParseExact(record.TransactionDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture),
                    Description = record.Description,
                    Amount = ParseAmount(record.Amount),
                    ChequeNumber = record.ChequeNumber,
                    Balance = ParseAmount(record.Balance),
                    TransactionType = ParseAmount(record.Amount) >= 0 ? "CR" : "DR"
                };

                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    private DateTime GetCellDate(ICell? cell)
    {
        if (cell == null)
            return DateTime.UtcNow;

        if (cell.CellType == CellType.Numeric)
            return cell.DateCellValue;

        if (cell.CellType == CellType.String && DateTime.TryParse(cell.StringCellValue, out var result))
            return result;

        return DateTime.UtcNow;
    }

    private decimal GetCellDecimal(ICell? cell)
    {
        if (cell == null)
            return 0;

        if (cell.CellType == CellType.Numeric)
            return (decimal)cell.NumericCellValue;

        return 0;
    }

    private string GetCellString(ICell? cell)
    {
        return cell?.StringCellValue ?? string.Empty;
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

public class YesBankTransactionRecord
{
    public string TransactionDate { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string ChequeNumber { get; set; } = string.Empty;
    public string Balance { get; set; } = string.Empty;
}

public sealed class YesBankTransactionMap : ClassMap<YesBankTransactionRecord>
{
    public YesBankTransactionMap()
    {
        Map(m => m.TransactionDate).Name("Transaction Date");
        Map(m => m.Description).Name("Description");
        Map(m => m.Amount).Name("Amount");
        Map(m => m.ChequeNumber).Name("Cheque Number");
        Map(m => m.Balance).Name("Balance");
    }
}
