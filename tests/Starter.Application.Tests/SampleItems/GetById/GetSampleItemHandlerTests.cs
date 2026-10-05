using Starter.Application.SampleItems;
using Starter.Application.SampleItems.GetById;
using Starter.Application.Tests.Setup;
using Starter.Domain.SampleItems;
using Microsoft.EntityFrameworkCore;

namespace Starter.Application.Tests.SampleItems.GetById;

[Collection(IntegrationTestCollection.Name)]
public sealed class GetSampleItemHandlerTests(PostgreSqlFixture fixture, HybridCacheTestFixture cacheFixture) : ApplicationIntegrationTest(fixture), IClassFixture<HybridCacheTestFixture>
{
    [Theory]
    [InlineData("TEST", "Test Entity")]
    public async Task HandleAsync_ShouldReturnSampleEntity_WhenEntityExists(string code, string name)
    {
        var entity = SampleItem.Create(name, new SampleItemCode(code));

        _dbContext.SampleItems.Add(entity);
        await _dbContext.SaveChangesAsync();

        var cache = cacheFixture.Cache;

        var handler = new GetSampleItemHandler(_dbContext, cache);

        var query = new GetSampleItemQuery(entity.Id);

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

        var handler = new GetSampleItemHandler(_dbContext, cache);

        var query = new GetSampleItemQuery(SampleItemId.New());

        var result = await handler.HandleAsync(query, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(SampleItemErrors.NotFound, result.Error);
    }

    [Theory]
    [InlineData("TEST", "Test Entity")]
    public async Task HandleAsync_ShouldReturnCachedEntity_OnSecondRequest(string code, string name)
    {
        var entity = SampleItem.Create(name, new SampleItemCode(code));

        _dbContext.SampleItems.Add(entity);
        await _dbContext.SaveChangesAsync();

        var cache = cacheFixture.Cache;

        var handler = new GetSampleItemHandler(_dbContext, cache);
        var query = new GetSampleItemQuery(entity.Id);

        var firstResult = await handler.HandleAsync(query, CancellationToken.None);

        _dbContext.SampleItems.Remove(entity);
        await _dbContext.SaveChangesAsync();

        var secondResult = await handler.HandleAsync(query, CancellationToken.None);

        Assert.True(firstResult.IsSuccess);
        Assert.True(secondResult.IsSuccess);
        Assert.Equal(entity.Id, secondResult.Value.Id);
    }
}
