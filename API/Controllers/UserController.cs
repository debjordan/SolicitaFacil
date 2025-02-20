using Microsoft.AspNetCore.Mvc;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.UserDTOs;

namespace SolicitaFacil.API.Controllers;

[ApiController]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserListDto>>> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserListDto>> GetUserByIdAsync(Guid id)
    {
        var result = await _userService.GetByIdUserAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserDto>> CreateUserAsync([FromBody] CreateUserDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _userService.CreateUserAsync(request);
        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _userService.UpdateUserAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}