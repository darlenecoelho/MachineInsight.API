using Bogus;
using MachineInsight.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace MachineInsight.Tests;

public class TestConfig<TService> where TService : class
{
    public Faker _faker;

    protected Mock<ILogger<TService>> Logger { get; }

    protected IConfigurationRoot Configuration { get; }

    protected Mock<IMachineRepository> MachineRepository { get; }

    public TestConfig()
    {
        _faker = new Faker();
        Logger = new Mock<ILogger<TService>>();
        Configuration = new ConfigurationBuilder().Build();
        MachineRepository = new Mock<IMachineRepository>();
    }
}