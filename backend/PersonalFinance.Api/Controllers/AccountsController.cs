using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Api.Services;
using PersonalFinance.Domain.DTOs;

namespace PersonalFinance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IBankAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IBankAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAllAccounts()
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching accounts");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccountDto>> GetAccountById(int id)
    {
        try
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
                return NotFound($"Account with ID {id} not found");

            return Ok(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching account");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<BankAccountDto>>> GetAccountsByMember(int memberId)
    {
        try
        {
            var accounts = await _accountService.GetAccountsByMemberAsync(memberId);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching accounts for member");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<BankAccountDto>> CreateAccount([FromBody] CreateAccountRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.BankName) || 
                string.IsNullOrWhiteSpace(request.AccountType) || 
                string.IsNullOrWhiteSpace(request.AccountNumber))
                return BadRequest("BankName, AccountType, and AccountNumber are required");

            var account = await _accountService.CreateAccountAsync(
                request.MemberId,
                request.BankName,
                request.AccountType,
                request.AccountNumber,
                request.AccountHolder,
                request.CurrentBalance
            );

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BankAccountDto>> UpdateAccount(int id, [FromBody] UpdateAccountRequest request)
    {
        try
        {
            var account = await _accountService.UpdateAccountAsync(id, request.CurrentBalance);
            return Ok(account);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Account with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        try
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class CreateAccountRequest
{
    public int MemberId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? AccountHolder { get; set; }
    public decimal CurrentBalance { get; set; }
}

public class UpdateAccountRequest
{
    public decimal CurrentBalance { get; set; }
}
