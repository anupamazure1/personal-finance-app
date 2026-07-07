using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Api.Services;

namespace PersonalFinance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IBankAccountService _accountService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, IBankAccountService accountService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet("account/{accountId}/{bank}")]
    public async Task<IActionResult> GetTransactionsByAccount(int accountId, string bank)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByAccountAsync(accountId, bank);
            return Ok(transactions);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("hdfc")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadHdfcStatement([FromForm] int accountId, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            // Verify account exists
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return NotFound($"Account with ID {accountId} not found");

            using (var stream = file.OpenReadStream())
            {
                var count = await _transactionService.UploadHdfcStatementAsync(accountId, stream, file.FileName);
                return Ok(new { message = $"Successfully imported {count} HDFC transactions", transactionCount = count });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading HDFC statement");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("icici")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadIciciStatement([FromForm] int accountId, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return NotFound($"Account with ID {accountId} not found");

            using (var stream = file.OpenReadStream())
            {
                var count = await _transactionService.UploadIciciStatementAsync(accountId, stream, file.FileName);
                return Ok(new { message = $"Successfully imported {count} ICICI transactions", transactionCount = count });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading ICICI statement");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("kotak")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadKotakStatement([FromForm] int accountId, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return NotFound($"Account with ID {accountId} not found");

            using (var stream = file.OpenReadStream())
            {
                var count = await _transactionService.UploadKotakStatementAsync(accountId, stream, file.FileName);
                return Ok(new { message = $"Successfully imported {count} Kotak transactions", transactionCount = count });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading Kotak statement");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("yesbank")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadYesBankStatement([FromForm] int accountId, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided");

            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
                return NotFound($"Account with ID {accountId} not found");

            using (var stream = file.OpenReadStream())
            {
                var count = await _transactionService.UploadYesBankStatementAsync(accountId, stream, file.FileName);
                return Ok(new { message = $"Successfully imported {count} Yes Bank transactions", transactionCount = count });
            }
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading Yes Bank statement");
            return StatusCode(500, "Internal server error");
        }
    }
}
