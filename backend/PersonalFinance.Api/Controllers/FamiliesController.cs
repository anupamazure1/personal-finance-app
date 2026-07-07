using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Api.Services;
using PersonalFinance.Domain.DTOs;

namespace PersonalFinance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamiliesController : ControllerBase
{
    private readonly IFamilyService _familyService;
    private readonly ILogger<FamiliesController> _logger;

    public FamiliesController(IFamilyService familyService, ILogger<FamiliesController> logger)
    {
        _familyService = familyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FamilyDto>>> GetAllFamilies()
    {
        try
        {
            var families = await _familyService.GetAllFamiliesAsync();
            return Ok(families);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching families");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FamilyDto>> GetFamilyById(int id)
    {
        try
        {
            var family = await _familyService.GetFamilyByIdAsync(id);
            if (family == null)
                return NotFound($"Family with ID {id} not found");

            return Ok(family);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching family");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<FamilyDto>> CreateFamily([FromBody] CreateFamilyRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Family name is required");

            var family = await _familyService.CreateFamilyAsync(request.Name, request.Description);
            return CreatedAtAction(nameof(GetFamilyById), new { id = family.Id }, family);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating family");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FamilyDto>> UpdateFamily(int id, [FromBody] UpdateFamilyRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Family name is required");

            var family = await _familyService.UpdateFamilyAsync(id, request.Name, request.Description);
            return Ok(family);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Family with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating family");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFamily(int id)
    {
        try
        {
            await _familyService.DeleteFamilyAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting family");
            return StatusCode(500, "Internal server error");
        }
    }
}

public class CreateFamilyRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateFamilyRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
