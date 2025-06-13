using MachineInsight.Application.DTOs;
using MachineInsight.Application.Interfaces;
using MachineInsight.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MachineInsight.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IMachineService _machineService;
    private readonly ILogger<MachinesController> _logger;

    public MachinesController(
        IMachineService machineService,
        ILogger<MachinesController> logger)
    {
        _machineService = machineService;
        _logger = logger;
    }

    /// <summary>
    /// Return list all Machines.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ResponseMachineDto>), 200)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll called");
        var list = await _machineService.GetAllAsync();
        _logger.LogInformation("GetAll returned {Count} machines", list.Count());
        return Ok(list);
    }

    /// <summary>
    /// Find Machine by id.
    /// </summary>
    [HttpGet("{id:guid}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(ResponseMachineDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("GetById called: MachineId={Id}", id);
        var machine = await _machineService.GetByIdAsync(id);
        if (machine is null)
        {
            _logger.LogWarning("GetById: MachineId={Id} not found", id);
            return NotFound($"Machine with ID '{id}' not found.");
        }

        _logger.LogInformation("GetById: MachineId={Id} found", id);
        return Ok(machine);
    }

    /// <summary>
    /// Find Machine by status.
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<ResponseMachineDto>), 200)]
    public async Task<IActionResult> GetByStatus(MachineStatus status)
    {
        _logger.LogInformation("GetByStatus called: Status={Status}", status);
        var list = await _machineService.GetByStatusAsync(status);
        _logger.LogInformation("GetByStatus returned {Count} machines", list.Count());
        return Ok(list);
    }

    /// <summary>
    /// Register new Machine.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseMachineDto), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> Create([FromBody] CreateMachineDto request)
    {
        _logger.LogInformation(
            "Create called: Name={Name}, Lat={Lat}, Lon={Lon}, Status={Status}",
            request.Name, request.Latitude, request.Longitude, request.Status);

        var result = await _machineService.CreateAsync(request);

        if (!result.Success)
        {
            _logger.LogWarning("Create failed: {Message}", result.Message);
            return BadRequest(result.Message);
        }

        _logger.LogInformation("Create succeeded: MachineId={Id}", result.Data!.Id);
        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data.Id },
            result.Data
        );
    }
    /// <summary>
    /// Update telemetry (status + rpm) of a Machine.
    /// </summary>
    [HttpPut("{id:guid}/telemetry")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTelemetry(Guid id, [FromBody] UpdateTelemetryDto request)
    {
        _logger.LogInformation(
            "UpdateTelemetry called: MachineId={Id}, Status={Status}, Rpm={Rpm}",
            id, request.Status, request.Rpm);

        var existing = await _machineService.GetByIdAsync(id);
        if (existing is null)
        {
            _logger.LogWarning("UpdateTelemetry: MachineId={Id} not found", id);
            return NotFound($"Machine with ID '{id}' not found.");
        }

        await _machineService.UpdateTelemetryAsync(id, request);
        _logger.LogInformation("Update machinie succeeded: MachineId={Id}", id);
        return Ok(new { message = "Machine updated successfully." });
    }

    /// <summary>
    /// Update the name and localization of the Machine.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResponseMachineDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMachineDto request)
    {
        _logger.LogInformation(
            "Controller.Update called: MachineId={Id}, Name={Name}, Lat={Lat}, Lon={Lon}",
            id, request.Name, request.Latitude, request.Longitude);

        ResponseMachineDto? updated;
        try
        {
            updated = await _machineService.UpdateAsync(id, request);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Controller.Update failed: {Msg}", ex.Message);
            return BadRequest(ex.Message);
        }

        if (updated is null)
        {
            _logger.LogWarning("Controller.Update: MachineId={Id} not found", id);
            return NotFound($"Machine with ID '{id}' not found.");
        }

        _logger.LogInformation("Controller.Update succeeded: MachineId={Id}", id);
        return Ok(updated);
    }

    /// <summary>
    /// Delete the Machine.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Delete called: MachineId={Id}", id);

        var deleted = await _machineService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Delete: MachineId={Id} not found", id);
            return NotFound($"Machine with ID '{id}' not found.");
        }

        _logger.LogInformation("Delete succeeded: MachineId={Id}", id);
        return Ok(new { message = "Machine deleted successfully." });
    }
}