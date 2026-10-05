using Starter.Application.ReportingEntities;
using Starter.Application.ReportingEntities.Create;
using Starter.Application.Tests.Setup;
using Starter.Domain.ReportingEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Starter.Application.Tests.ReportingEntities.Create;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateReportingEntityHandlerTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public async Task HandleAsync_ShouldPersistReportingEntity(string name, string codeValue)
    {
        var command = new CreateReportingEntityCommand(name, new ReportingEntityCode(codeValue));

        var handler = new CreateReportingEntityHandler(_dbContext, NullLogger<CreateReportingEntityHandler>.Instance);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var reportingEntity = await _dbContext.ReportingEntities.SingleAsync(x => x.Id == result.Value.Id);

        Assert.Equal(name, reportingEntity.Name);
        Assert.Equal(command.Code, reportingEntity.Code);
    }

    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public async Task HandleAsync_WhenCodeAlreadyExists_ShouldReturnFailure(string name, string codeValue)
    {
        var command = new CreateReportingEntityCommand(name, new ReportingEntityCode(codeValue));

        var handler = new CreateReportingEntityHandler(_dbContext, NullLogger<CreateReportingEntityHandler>.Instance);

        var firstResult = await handler.HandleAsync(command, CancellationToken.None);
        var secondResult = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(firstResult.IsSuccess);
        Assert.True(secondResult.IsFailure);
        Assert.Equal(ReportingEntityErrors.AlreadyExists, secondResult.Error);
    }
}
