using Microsoft.AspNetCore.Mvc;
using SolicitaFacil.API.DTOs.ServiceDTOs;
using SolicitaFacil.Domain.Entities;

namespace SolicitaFacil.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceController : ControllerBase
{
    private readonly IServiceRepository _iserviceRepository;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceListDto>>> GetAllServicesAsync()
    {
        var result = await _iserviceRepository.GetAllServicesAsync();
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceListDto>> GetServiceByIdAsync(ServiceListDto request)
    {
        var result = await _iserviceRepository.GetServiceByIdAsync(request.ServiceId);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CreateServiceDto>> CreateServiceAsync([FromBody] CreateServiceDto request)
    {
        var result = await _iserviceRepository.CreateServiceAsync(request);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateServiceDto>> UpdateServiceAsync([FromBody] UpdateServiceDto request)
    {
        var result = await _iserviceRepository.UpdateServiceAsync(request.ServiceId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteServiceDto>> DeleteServiceAsync(DeleteServiceDto request)
    {
        var result = await _iserviceRepository.DeleteServiceAsync(request.ServiceId);
        return Ok(result);
    }
}