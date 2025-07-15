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
        catch (Exception ex) when (ex is ArgumentException || ex is HttpRequestException || ex is InvalidOperationException)
        {
            _logger.LogError("Bad request: {er}", ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest, new { data = default(User), error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {er}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { data = default(User), error = ex.Message });
        }
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> TransferAsync([FromBody] Deposit deposit)
    {
        try
        {
            await _service.DepositAsync(deposit);

            return Ok(new { error = string.Empty });
        }
        catch (Exception ex) when (ex is ArgumentException || ex is HttpRequestException || ex is InvalidOperationException)
        {
            _logger.LogError("Bad request: {er}", ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest, new { data = default(Deposit), error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {er}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { data = default(Deposit), error = ex.Message });
        }
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferAsync([FromBody]Transfer transfer)
    {
        try
        {
            await _service.TransferAsync(transfer.Value, transfer.Payer, transfer.Payee);

            return Ok(new { error = string.Empty });
        }
        catch (Exception ex) when (ex is ArgumentException || ex is HttpRequestException || ex is InvalidOperationException)
        {
            _logger.LogError("Bad request: {er}", ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest, new { data = default(Transfer), error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {er}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { data = default(Transfer), error = ex.Message });
        }
    }
}
