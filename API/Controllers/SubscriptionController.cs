using Microsoft.AspNetCore.Mvc;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;

namespace SolicitaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriptionDetailsDto>>> GetAllSubscriptionsAsync()
    {
        var result = await _subscriptionService.GetAllSubscriptionsAsync();
        if (result == null)
        {
            NoContent();
        }
        return Ok(result);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SubscriptionDetailsDto>> GetSubscriptionByIdAsync(Guid Id)
    {
        var result = await _subscriptionService.GetSubscriptionByIdAsync(Id);
        if (result == null)
        {
            NotFound(new { Message = $"User with ID {Id} not found"});
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateSubscriptionDto>> CreateSubscriptionAsync([FromBody] CreateSubscriptionDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }   
                
        var result = await _subscriptionService.CreateSubscriptionAsync(request);
        if (result == null)
        {
            return Conflict(new { Message = $"Subscription with ID {request.UserId} already exists" });
        }
        
        return CreatedAtAction(nameof(GetSubscriptionByIdAsync), new { id = result.UserId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateSubscriptionDto>> UpdateSubscriptionByIdAsync(Guid id, [FromBody] UpdateSubscriptionDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var exist = await GetSubscriptionByIdAsync(id);
        if (exist == null)
        {
            return NotFound(new { Message = $"Subscription with ID {id} not found" });
        }
        
        await _subscriptionService.UpdateSubscriptionByIdAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<CancelSubscriptionDto>> DeleteSubscriptionByIdAsync(Guid id)
    {
        var exist = await GetSubscriptionByIdAsync(id);
        if (exist == null)
        {
            return NotFound(new { Message = $"Subscription with ID {id} not found" });
        }
        
        await _subscriptionService.DeleteSubscriptionByIdAsync(id);
        return NoContent();
    }
}