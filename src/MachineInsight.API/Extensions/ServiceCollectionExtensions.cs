using FluentValidation;
using FluentValidation.AspNetCore;
using MachineInsight.API.Services;
using MachineInsight.API.Settings;
using MachineInsight.Application.Interfaces;
using MachineInsight.Application.Mappings;
using MachineInsight.Application.Services;
using MachineInsight.Application.Validators;
using MachineInsight.Domain.Interfaces;
using MachineInsight.Infrastructure.Persistence;
using MachineInsight.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MachineInsight.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<CreateMachineDtoValidator>();
        return services;
    }

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddScoped<IMachineService, MachineService>();
        return services;
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MachineInsightDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMachineRepository, MachineRepository>();

        services.Configure<SimulatorOptions>(
            config.GetSection("TelemetrySimulator"));

        return services;
    }

    public static IServiceCollection AddRealtimeAndHostedServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddHostedService<TelemetrySimulatorService>();
        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "Machine.Insight API", Version = "v1" });
        });
        return services;
    }
}
