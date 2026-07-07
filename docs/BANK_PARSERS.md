# Bank Statement Parsers

This document describes the bank statement parser implementations for each supported bank.

## Overview

Each bank parser implements the `IBankStatementParser` interface and is responsible for:
1. Validating the uploaded file format
2. Parsing transaction data from the bank statement
3. Mapping bank-specific fields to standardized transaction format
4. Handling date and amount format conversions

## HDFC Bank Parser

### Supported Formats
- CSV export from HDFC NetBanking
- PDF statements (future enhancement)

### Expected CSV Structure
```
Transaction Date,Value Date,Cheque #,Reference #,Description,Amount,Balance
01-Jan-2024,01-Jan-2024,,REF12345,Fund Transfer,-5000.00,95000.00
```

### Implementation
```csharp
namespace PersonalFinance.Core.Parsers
{
    public class HdfcBankParser : IBankStatementParser
    {
        public async Task<List<BankTransaction>> ParseAsync(Stream fileStream)
        {
            var transactions = new List<BankTransaction>();
            // Parse CSV and extract transactions
            return transactions;
        }
    }
}
```

### Mapping
- Transaction Date → TransactionDate
- Value Date → ValueDate
- Description → Description
- Amount → Amount (negative for debits)
- Cheque # → ReferenceNumber
- Balance → EndingBalance

## ICICI Bank Parser

### Supported Formats
- Excel (.xlsx) export from ICICI iConnect
- CSV format
- PDF statements (future enhancement)

### Expected Excel Structure
```
Transaction Date | Amount | Description | Cheque Number | Balance
01-01-2024       | 5000   | Salary      | -             | 105000
```

### Implementation
```csharp
public class IciciBankParser : IBankStatementParser
{
    public async Task<List<BankTransaction>> ParseAsync(Stream fileStream)
    {
        var transactions = new List<BankTransaction>();
        // Parse Excel/CSV and extract transactions
        return transactions;
    }
}
```

### Mapping
- Transaction Date → TransactionDate
- Amount → Amount
- Description → Description
- Cheque Number → ReferenceNumber
- Balance → EndingBalance

## Kotak Bank Parser

### Supported Formats
- CSV export from Kotak NetBanking
- PDF statements (future enhancement)

### Expected CSV Structure
```
Txn Date,Particulars,Cheque #,Amount,Balance
01-01-2024,Fund Transfer,CH001,-5000,95000
```

### Implementation
```csharp
public class KotakBankParser : IBankStatementParser
{
    public async Task<List<BankTransaction>> ParseAsync(Stream fileStream)
    {
        var transactions = new List<BankTransaction>();
        // Parse CSV and extract transactions
        return transactions;
    }
}
```

### Mapping
- Txn Date → TransactionDate
- Particulars → Description
- Amount → Amount
- Cheque # → ReferenceNumber
- Balance → EndingBalance

## Yes Bank Parser

### Supported Formats
- Excel (.xlsx) export from Yes Bank NetBanking
- CSV format
- PDF statements (future enhancement)

### Expected Excel Structure
```
Transaction Date | Description | Amount | Cheque Number | Balance
01-01-2024       | Salary      | 5000   | -             | 105000
```

### Implementation
```csharp
public class YesBankParser : IBankStatementParser
{
    public async Task<List<BankTransaction>> ParseAsync(Stream fileStream)
    {
        var transactions = new List<BankTransaction>();
        // Parse Excel/CSV and extract transactions
        return transactions;
    }
}
```

### Mapping
- Transaction Date → TransactionDate
- Description → Description
- Amount → Amount
- Cheque Number → ReferenceNumber
- Balance → EndingBalance

## Parser Interface

```csharp
public interface IBankStatementParser
{
    Task<List<BankTransaction>> ParseAsync(Stream fileStream);
    bool ValidateFormat(Stream fileStream);
}
```

## Common Patterns

### Date Format Handling
Each parser handles bank-specific date formats:
```csharp
private DateTime ParseDate(string dateString)
{
    // Handle DD-MMM-YYYY, DD/MM/YYYY, etc.
    return DateTime.ParseExact(dateString, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
}
```

### Amount Format Handling
```csharp
private decimal ParseAmount(string amountString)
{
    // Handle negative signs, spaces, different decimal separators
    var cleaned = amountString.Replace(" ", "").Replace(",", "");
    return decimal.Parse(cleaned, CultureInfo.InvariantCulture);
}
```

### Duplicate Detection
```csharp
private bool IsDuplicateTransaction(BankTransaction transaction, List<BankTransaction> existing)
{
    return existing.Any(t => 
        t.TransactionDate == transaction.TransactionDate &&
        t.Amount == transaction.Amount &&
        t.Description == transaction.Description);
}
```

## Error Handling

### Custom Exceptions
```csharp
public class InvalidBankStatementException : Exception
{
    public InvalidBankStatementException(string message) : base(message) { }
}

public class UnrecognizedFormatException : Exception
{
    public UnrecognizedFormatException(string message) : base(message) { }
}
```

### Validation
```csharp
public class StatementValidator
{
    public static ValidationResult Validate(List<BankTransaction> transactions)
    {
        if (transactions.Count == 0)
            throw new InvalidBankStatementException("No transactions found in statement");
        
        // Additional validation logic
        return new ValidationResult { IsValid = true };
    }
}
```

## Adding a New Bank Parser

1. Create a new parser class implementing `IBankStatementParser`
2. Implement `ParseAsync()` method
3. Register in dependency injection container
4. Add to parser factory
5. Create unit tests
6. Update documentation

## Testing

```csharp
[TestClass]
public class HdfcBankParserTests
{
    [TestMethod]
    public async Task ParseAsync_ValidCsv_ReturnsTransactions()
    {
        // Arrange
        var parser = new HdfcBankParser();
        var fileStream = GetTestFileStream();
        
        // Act
        var result = await parser.ParseAsync(fileStream);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0);
    }
}
```
