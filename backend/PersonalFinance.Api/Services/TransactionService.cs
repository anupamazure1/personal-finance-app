namespace PersonalFinance.Api.Services;

using PersonalFinance.Core.Parsers;
using PersonalFinance.Data.Repositories;
using PersonalFinance.Domain.DTOs;
using PersonalFinance.Domain.Entities;

public interface ITransactionService
{
    Task<IEnumerable<BankTransactionDto>> GetTransactionsByAccountAsync(int accountId, string bank);
    Task<int> UploadHdfcStatementAsync(int accountId, Stream fileStream, string fileName);
    Task<int> UploadIciciStatementAsync(int accountId, Stream fileStream, string fileName);
    Task<int> UploadKotakStatementAsync(int accountId, Stream fileStream, string fileName);
    Task<int> UploadYesBankStatementAsync(int accountId, Stream fileStream, string fileName);
}

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly BankParserFactory _parserFactory;

    public TransactionService(ITransactionRepository transactionRepository, BankParserFactory parserFactory)
    {
        _transactionRepository = transactionRepository;
        _parserFactory = parserFactory;
    }

    public async Task<IEnumerable<BankTransactionDto>> GetTransactionsByAccountAsync(int accountId, string bank)
    {
        var transactions = bank.ToLower() switch
        {
            "hdfc" => await _transactionRepository.GetHdfcTransactionsByAccountAsync(accountId),
            "icici" => await _transactionRepository.GetIciciTransactionsByAccountAsync(accountId),
            "kotak" => await _transactionRepository.GetKotakTransactionsByAccountAsync(accountId),
            "yesbank" => await _transactionRepository.GetYesBankTransactionsByAccountAsync(accountId),
            _ => throw new ArgumentException($"Unsupported bank: {bank}")
        };

        return transactions.Select(t => new BankTransactionDto
        {
            Id = t.Id,
            AccountId = t.AccountId,
            TransactionDate = t.TransactionDate,
            ValueDate = t.ValueDate,
            Description = t.Description,
            Amount = t.Amount,
            Balance = t.Balance,
            ReferenceNumber = t.ReferenceNumber,
            ChequeNumber = t.ChequeNumber,
            Category = t.Category,
            Bank = bank
        });
    }

    public async Task<int> UploadHdfcStatementAsync(int accountId, Stream fileStream, string fileName)
    {
        return await UploadStatementAsync(accountId, fileStream, fileName, "hdfc");
    }

    public async Task<int> UploadIciciStatementAsync(int accountId, Stream fileStream, string fileName)
    {
        return await UploadStatementAsync(accountId, fileStream, fileName, "icici");
    }

    public async Task<int> UploadKotakStatementAsync(int accountId, Stream fileStream, string fileName)
    {
        return await UploadStatementAsync(accountId, fileStream, fileName, "kotak");
    }

    public async Task<int> UploadYesBankStatementAsync(int accountId, Stream fileStream, string fileName)
    {
        return await UploadStatementAsync(accountId, fileStream, fileName, "yesbank");
    }

    private async Task<int> UploadStatementAsync(int accountId, Stream fileStream, string fileName, string bankName)
    {
        var parser = _parserFactory.GetParser(bankName)
            ?? throw new InvalidOperationException($"No parser found for bank: {bankName}");

        if (!parser.SupportsFormat(fileName))
            throw new InvalidOperationException($"File format not supported for {bankName}");

        var parsedTransactions = await parser.ParseAsync(fileStream, fileName);
        
        // Set account ID for all transactions
        foreach (var transaction in parsedTransactions)
        {
            transaction.AccountId = accountId;
        }

        // Add transactions based on bank type
        switch (bankName.ToLower())
        {
            case "hdfc":
                var hdfcTransactions = parsedTransactions.OfType<HdfcBankTransaction>().ToList();
                await _transactionRepository.AddHdfcTransactionsAsync(hdfcTransactions);
                return hdfcTransactions.Count;

            case "icici":
                var iciciTransactions = parsedTransactions.OfType<IciciBankTransaction>().ToList();
                await _transactionRepository.AddIciciTransactionsAsync(iciciTransactions);
                return iciciTransactions.Count;

            case "kotak":
                var kotakTransactions = parsedTransactions.OfType<KotakBankTransaction>().ToList();
                await _transactionRepository.AddKotakTransactionsAsync(kotakTransactions);
                return kotakTransactions.Count;

            case "yesbank":
                var yesBankTransactions = parsedTransactions.OfType<YesBankTransaction>().ToList();
                await _transactionRepository.AddYesBankTransactionsAsync(yesBankTransactions);
                return yesBankTransactions.Count;

            default:
                throw new InvalidOperationException($"Unknown bank: {bankName}");
        }
    }
}
