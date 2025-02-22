using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using System.Threading;

namespace SolicitaFacil.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Produces("application/json")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;
    private readonly IValidator<CreateSubscriptionDto> _createValidator;
    private readonly IValidator<UpdateSubscriptionDto> _updateValidator;

    public SubscriptionController(
        ISubscriptionService subscriptionService,
        ILogger<SubscriptionController> logger,
        IValidator<CreateSubscriptionDto> createValidator,
        IValidator<UpdateSubscriptionDto> updateValidator)
    {
        _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all subscriptions.");
        var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync(cancellationToken);

        if (subscriptions == null || !subscriptions.Any())
        {
            _logger.LogWarning("No subscriptions found.");
            return NotFound(new ApiResponse<object>(false, "No subscriptions found."));
        }

        return Ok(new ApiResponse<IEnumerable<SubscriptionDetailsDto>>(true, "Subscriptions retrieved successfully.", subscriptions));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching subscription with ID {SubscriptionId}.", id);
        var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id, cancellationToken);

        if (subscription == null)
        {
            _logger.LogWarning("Subscription with ID {SubscriptionId} not found.", id);
            return NotFound(new ApiResponse<object>(false, $"Subscription with ID {id} not found."));
        }

        return Ok(new ApiResponse<SubscriptionDetailsDto>(true, "Subscription retrieved successfully.", subscription));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateSubscriptionAsync(
        [FromBody] CreateSubscriptionDto request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating a new subscription for user {UserId}.", request.UserId);

        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for subscription creation: {Errors}", validationResult.Errors);
            return BadRequest(new ApiResponse<object>(false, "Validation failed.", validationResult.Errors));
        }

        var result = await _subscriptionService.CreateSubscriptionAsync(request, cancellationToken);
        if (result == null)
        {
            _logger.LogWarning("Subscription creation failed: already exists for user {UserId}.", request.UserId);
            return Conflict(new ApiResponse<object>(false, $"A subscription for user {request.UserId} already exists."));
        }

        return CreatedAtAction(
            nameof(GetSubscriptionByIdAsync),
            new { id = result.SubscriptionId ?? result.UserId, version = "1.0" },
            new ApiResponse<CreateSubscriptionDto>(true, "Subscription created successfully.", result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSubscriptionByIdAsync(
        Guid id,
        [FromBody] UpdateSubscriptionDto request,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating subscription with ID {SubscriptionId}.", id);

        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for subscription update: {Errors}", validationResult.Errors);
            return BadRequest(new ApiResponse<object>(false, "Validation failed.", validationResult.Errors));
        }

        if (request.SubscriptionId != null && request.SubscriptionId != id)
        {
            _logger.LogWarning("Subscription ID mismatch: {RequestId} vs {RouteId}", request.SubscriptionId, id);
            return BadRequest(new ApiResponse<object>(false, "Subscription ID in request does not match the route ID."));
        }

        var updated = await _subscriptionService.UpdateSubscriptionByIdAsync(id, request, cancellationToken);
        if (!updated)
        {
            _logger.LogWarning("Subscription with ID {SubscriptionId} not found for update.", id);
            return NotFound(new ApiResponse<object>(false, $"Subscription with ID {id} not found."));
        }

        _logger.LogInformation("Subscription with ID {SubscriptionId} updated successfully.", id);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting subscription with ID {SubscriptionId}.", id);

        var deleted = await _subscriptionService.DeleteSubscriptionByIdAsync(id, cancellationToken);
        if (!deleted)
        {
            _logger.LogWarning("Subscription with ID {SubscriptionId} not found for deletion.", id);
            return NotFound(new ApiResponse<object>(false, $"Subscription with ID {id} not found."));
        }

        _logger.LogInformation("Subscription with ID {SubscriptionId} deleted successfully.", id);
        return NoContent();
    }
}

public record ApiResponse<T>(bool Success, string Message, T Data = default, object Errors = null);