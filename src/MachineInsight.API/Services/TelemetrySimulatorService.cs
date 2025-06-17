using MachineInsight.API.Hubs;
using MachineInsight.API.Settings;
using MachineInsight.Application.DTOs;
using MachineInsight.Application.Interfaces;
using MachineInsight.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace MachineInsight.API.Services;

public class TelemetrySimulatorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<TelemetryHub> _hubContext;
    private readonly ILogger<TelemetrySimulatorService> _logger;
    private readonly Random _random = new();
    private readonly TimeSpan _interval;

    public TelemetrySimulatorService(
        IServiceProvider serviceProvider,
        IHubContext<TelemetryHub> hubContext,
        IOptions<SimulatorOptions> opts,
        ILogger<TelemetrySimulatorService> logger)
    {
        _serviceProvider = serviceProvider;
        _hubContext = hubContext;
        _logger = logger;
        _interval = TimeSpan.FromSeconds(opts.Value.IntervalSeconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Telemetry simulator started.");

        var values = Enum.GetValues<MachineStatus>()
                         .Where(s => s != MachineStatus.Unknown)
                         .ToArray();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var machineService = scope.ServiceProvider.GetRequiredService<IMachineService>();
                var machines = await machineService.GetAllAsync();

                if (machines.Any())
                {
                    var list = machines.ToList();
                    var countToUpdate = Math.Min(3, list.Count);
                    var selectedMachines = list
                        .OrderBy(_ => _random.Next())
                        .Take(countToUpdate)
                        .ToList();

                    foreach (var selected in selectedMachines)
                    {
                        var telemetry = new UpdateTelemetryDto
                        {
                            Status = values[_random.Next(values.Length)],
                            Rpm = _random.Next(500, 3000)
                        };

                        await machineService.UpdateTelemetryAsync(selected.Id, telemetry);

                        await _hubContext.Clients.All.SendAsync("ReceiveTelemetry", new
                        {
                            MachineId = selected.Id,
                            telemetry.Status,
                            telemetry.Rpm,
                            Timestamp = DateTime.UtcNow
                        });

                        _logger.LogInformation(
                            "Simulated telemetry for {MachineId}: Status={Status}, RPM={Rpm}",
                            selected.Id, telemetry.Status, telemetry.Rpm
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while simulating telemetry");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
