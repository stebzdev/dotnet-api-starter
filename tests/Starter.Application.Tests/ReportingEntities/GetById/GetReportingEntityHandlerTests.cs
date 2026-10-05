using Starter.Application.ReportingEntities;
using Starter.Application.ReportingEntities.GetById;
using Starter.Application.Tests.Setup;
using Starter.Domain.ReportingEntities;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.ReportingEntities.GetById;

[Collection(IntegrationTestCollection.Name)]
public sealed class GetReportingEntityHandlerTests(PostgreSqlFixture fixture, HybridCacheTestFixture cacheFixture) : ApplicationIntegrationTest(fixture), IClassFixture<HybridCacheTestFixture>
{
    [Theory]
    [InlineData("TEST", "Test Entity")]
    public async Task HandleAsync_ShouldReturnReportingEntity_WhenEntityExists(string code, string name)
    {
        var entity = ReportingEntity.Create(name, new ReportingEntityCode(code));

        _dbContext.ReportingEntities.Add(entity);
        await _dbContext.SaveChangesAsync();

        var cache = cacheFixture.Cache;

        var handler = new GetReportingEntityHandler(_dbContext, cache);

        var query = new GetReportingEntityQuery(entity.Id);

        var result = await handler.HandleAsync(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(entity.Id, result.Value.Id);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(code, result.Value.Code.Value);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenEntityDoesNotExist()
    {
        var cache = cacheFixture.Cache;

        var handler = new GetReportingEntityHandler(_dbContext, cache);

        var query = new GetReportingEntityQuery(ReportingEntityId.New());

        var result = await handler.HandleAsync(query, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ReportingEntityErrors.NotFound, result.Error);
    }

    [Theory]
    [InlineData("TEST", "Test Entity")]
    public async Task HandleAsync_ShouldReturnCachedEntity_OnSecondRequest(string code, string name)
    {
        var entity = ReportingEntity.Create(name, new ReportingEntityCode(code));

        _dbContext.ReportingEntities.Add(entity);
        await _dbContext.SaveChangesAsync();

        var cache = cacheFixture.Cache;

        var handler = new GetReportingEntityHandler(_dbContext, cache);
        var query = new GetReportingEntityQuery(entity.Id);

        var firstResult = await handler.HandleAsync(query, CancellationToken.None);

        _dbContext.ReportingEntities.Remove(entity);
        await _dbContext.SaveChangesAsync();

        var secondResult = await handler.HandleAsync(query, CancellationToken.None);

        Assert.True(firstResult.IsSuccess);
        Assert.True(secondResult.IsSuccess);
        Assert.Equal(entity.Id, secondResult.Value.Id);
    }
}
