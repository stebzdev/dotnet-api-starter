using Starter.Application.ReportingEntities;
using Starter.Application.ReportingPeriods.Create;
using Starter.Application.Tests.Setup;
using Starter.Domain.ReportingEntities;
using Starter.Domain.ReportingPeriods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Starter.Application.Tests.ReportingPeriods.Create;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateReportingPeriodHandlerTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Theory]
    [InlineData("Acme Bank", "ACME", 2026, 9)]
    [InlineData("Global Finance", "GLOBAL", 2025, 12)]
    public async Task HandleAsync_ShouldPersistReportingPeriod(string entityName,  string entityCode, int referenceYear, int referenceMonth)
    {
        var reportingEntity = ReportingEntity.Create(entityName, new ReportingEntityCode(entityCode));

        _dbContext.ReportingEntities.Add(reportingEntity);
        await _dbContext.SaveChangesAsync();

        var command = new CreateReportingPeriodCommand(reportingEntity.Id, new ReferencePeriod(referenceYear, referenceMonth));

        var handler = new CreateReportingPeriodHandler(_dbContext, NullLogger<CreateReportingPeriodHandler>.Instance);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var reportingPeriod = await _dbContext.ReportingPeriods.SingleAsync(x => x.Id == new ReportingPeriodId(result.Value.Id.Value));

        Assert.Equal(command.ReportingEntityId, reportingPeriod.ReportingEntityId);
        Assert.Equal(command.ReferencePeriod, reportingPeriod.ReferencePeriod);
        Assert.Equal(ReportingStatus.Draft, reportingPeriod.Status);
    }

    [Theory]
    [InlineData("Acme Bank", "ACME", 2026, 9)]
    [InlineData("Global Finance", "GLOBAL", 2025, 12)]
    public async Task HandleAsync_WhenPeriodAlreadyExists_ShouldReturnFailure(string entityName, string entityCode, int referenceYear, int referenceMonth)
    {
        var reportingEntity = ReportingEntity.Create(entityName, new ReportingEntityCode(entityCode));

        _dbContext.ReportingEntities.Add(reportingEntity);
        await _dbContext.SaveChangesAsync();

        var command = new CreateReportingPeriodCommand(reportingEntity.Id, new ReferencePeriod(referenceYear, referenceMonth));

        var handler = new CreateReportingPeriodHandler(_dbContext, NullLogger<CreateReportingPeriodHandler>.Instance);

        var firstResult = await handler.HandleAsync(command, CancellationToken.None);
        var secondResult = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(firstResult.IsSuccess);
        Assert.True(secondResult.IsFailure);
        Assert.Equal(CreateReportingPeriodErrors.AlreadyExists, secondResult.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenReportingEntityDoesNotExist_ShouldReturnFailure()
    {
        var command = new CreateReportingPeriodCommand(ReportingEntityId.New(), new ReferencePeriod(2026, 9));

        var handler = new CreateReportingPeriodHandler(_dbContext, NullLogger<CreateReportingPeriodHandler>.Instance);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(CreateReportingPeriodErrors.ReportingEntityNotFound, result.Error);
    }


}
