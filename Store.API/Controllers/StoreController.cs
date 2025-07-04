using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace StoreAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StoreController(ILogger<StoreController> logger, IStoreService service) : ControllerBase
{
    private readonly IStoreService _service = service;
    private readonly ILogger<StoreController> _logger = logger;

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUserAsync([FromBody]User user)
    {
        try
        {
            var response = await _service.CreateUserAsync(user);

            return Ok(new { data = response, error = string.Empty });
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {er}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { data = default(User), error = ex.Message });
        }
    }
}
