using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Api.Services;
using PersonalFinance.Domain.DTOs;

namespace PersonalFinance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary()
    {
        try
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard summary");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("family/{familyId}/net-worth")]
    public async Task<IActionResult> GetFamilyNetWorth(int familyId)
    {
        try
        {
            var netWorth = await _dashboardService.GetFamilyNetWorthAsync(familyId);
            return Ok(new { familyId, netWorth });
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Family with ID {familyId} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching family net worth");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("member/{memberId}/net-worth")]
    public async Task<IActionResult> GetMemberNetWorth(int memberId)
    {
        try
        {
            var netWorth = await _dashboardService.GetMemberNetWorthAsync(memberId);
            return Ok(new { memberId, netWorth });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching member net worth");
            return StatusCode(500, "Internal server error");
        }
    }
}
