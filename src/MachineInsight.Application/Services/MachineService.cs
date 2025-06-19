using AutoMapper;
using MachineInsight.Application.DTOs;
using MachineInsight.Application.Interfaces;
using MachineInsight.Domain.Entities;
using MachineInsight.Domain.Enums;
using MachineInsight.Domain.Interfaces;
using MachineInsight.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MachineInsight.Application.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MachineService> _logger;


    public MachineService(IMachineRepository repository, IMapper mapper, ILogger<MachineService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;

    }

    public async Task<CreateMachineResponse> CreateAsync(CreateMachineDto createMachineDto)
    {
        if (await _repository.ExistsByNameAsync(createMachineDto.Name))
        {
            _logger.LogWarning("CreateAsync: Duplicate name '{Name}'", createMachineDto.Name);
            return new CreateMachineResponse
            {
                Success = false,
                Message = $"A machine with the name '{createMachineDto.Name}' already exists."
            };
        }

        try
        {
            var machine = new Machine(createMachineDto.Name,
                                      new Location(createMachineDto.Latitude, createMachineDto.Longitude),
                                      createMachineDto.Status);

            await _repository.AddAsync(machine);
            var machineDto = _mapper.Map<ResponseMachineDto>(machine);

            _logger.LogInformation("CreateAsync: Machine '{Name}' created successfully (Id={Id})",
                                   machine.Name, machine.Id);

            return new CreateMachineResponse
            {
                Success = true,
                Message = "Machine created successfully.",
                Data = machineDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateAsync: Error creating machine '{Name}'", createMachineDto.Name);
            return new CreateMachineResponse
            {
                Success = false,
                Message = "An error occurred while creating the machine."
            };
        }
    }

    public async Task<IEnumerable<ResponseMachineDto>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called");
        var machines = await _repository.GetAllAsync();

        var listMachine = _mapper.Map<IEnumerable<ResponseMachineDto>>(machines);
        var count = listMachine.Count();
        _logger.LogInformation("GetAllAsync returned {Count} machines", count);

        return listMachine;
    }

    public async Task<IEnumerable<ResponseMachineDto>> GetByStatusAsync(MachineStatus status)
    {
        _logger.LogInformation("GetByStatusAsync called with Status={Status}", status);
        var machines = await _repository.GetByStatusAsync(status);

        var machineResponseDto = _mapper.Map<IEnumerable<ResponseMachineDto>>(machines);
        var count = machineResponseDto.Count();
        _logger.LogInformation(
            "GetByStatusAsync returned {Count} machines for Status={Status}",
            count, status);

        return machineResponseDto;
    }

    public async Task<ResponseMachineDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("GetByIdAsync called with Id={Id}", id);
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
        {
            _logger.LogWarning("GetByIdAsync: Machine with Id={Id} not found", id);
            return null;
        }

        var machineResponseDto = _mapper.Map<ResponseMachineDto>(machine);
        _logger.LogInformation("GetByIdAsync: Machine with Id={Id} found", id);
        return machineResponseDto;
    }

    public async Task UpdateTelemetryAsync(Guid id, UpdateTelemetryDto telemetryUpdateDto)
    {
        _logger.LogInformation(
            "UpdateTelemetryAsync called: Id={Id}, Status={Status}, Rpm={Rpm}",
            id, telemetryUpdateDto.Status, telemetryUpdateDto.Rpm);

        var machine = await _repository.GetByIdAsync(id);
        if (machine is null)
        {
            _logger.LogWarning("UpdateTelemetryAsync: Machine with Id={Id} not found", id);
            return;
        }

        machine.UpdateTelemetry(telemetryUpdateDto.Status, telemetryUpdateDto.Rpm);
        await _repository.UpdateAsync(machine);

        _logger.LogInformation("UpdateTelemetryAsync succeeded for MachineId={MachineId}", id);
    }

    public async Task<ResponseMachineDto?> UpdateAsync(Guid id, UpdateMachineDto updateMachineDto)
    {
        _logger.LogInformation(
            "UpdateAsync called: Id={Id}, Name={Name}, Lat={Lat}, Lon={Lon}",
            id, updateMachineDto.Name, updateMachineDto.Latitude, updateMachineDto.Longitude);

        var machine = await _repository.GetByIdAsync(id);
        if (machine is null)
        {
            _logger.LogWarning("UpdateAsync: MachineId={Id} not found", id);
            return null;
        }

        if (await _repository.ExistsByNameAsync(updateMachineDto.Name))
        {
            _logger.LogWarning("UpdateAsync: Name '{Name}' is already taken", updateMachineDto.Name);
            throw new InvalidOperationException($"A machine with the name '{updateMachineDto.Name}' already exists.");
        }

        var location = new Location(updateMachineDto.Latitude, updateMachineDto.Longitude);
        machine.UpdateDetails(updateMachineDto.Name, location);

        await _repository.UpdateAsync(machine);

        var updatedDto = _mapper.Map<ResponseMachineDto>(machine);
        _logger.LogInformation("UpdateAsync succeeded: MachineId={Id}", id);
        return updatedDto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteAsync called with Id={Id}", id);

        var machine = await _repository.GetByIdAsync(id);
        if (machine is null)
        {
            _logger.LogWarning("DeleteAsync: Machine with Id={Id} not found", id);
            return false;
        }

        await _repository.DeleteAsync(machine);
        _logger.LogInformation("DeleteAsync succeeded: MachineId={Id} deleted", id);
        return true;
    }
}
