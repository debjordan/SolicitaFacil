using Microsoft.AspNetCore.Mvc;
using SolicitaFacil.Application.Interfaces;
using SolicitaFacil.Shared.DTOs.UserDTOs;

namespace SolicitaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        if (result == null)
        {
            return NoContent();
        }
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserListDto>> GetUserByIdAsync(Guid id)
    {
        var result = await _userService.GetByIdUserAsync(id);
        if (result == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }
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
    public async Task<ActionResult<UpdateUserDto>> UpdateUserAsync(Guid id, [FromBody] UpdateUserDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await _userService.GetByIdUserAsync(id);
        if (userExists == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }

        await _userService.UpdateUserAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        var userExists = await _userService.GetByIdUserAsync(id);
        if (userExists == null)
        {
            return NotFound(new { Message = $"User with ID {id} not found" });
        }
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}
