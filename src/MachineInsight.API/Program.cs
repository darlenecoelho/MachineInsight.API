using MachineInsight.API.Extensions;
using MachineInsight.API.Hubs;
using MachineInsight.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MachineInsightUI", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")  
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();                   
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddValidation();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services
    .AddControllers()
    .Services
    .AddRealtimeAndHostedServices();

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MachineInsightDbContext>();
db.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MachineInsight API V1"));
}

app.UseCors("MachineInsightUI");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();
