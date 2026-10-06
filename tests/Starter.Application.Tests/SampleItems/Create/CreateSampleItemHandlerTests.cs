using Starter.Application.SampleItems;
using Starter.Application.SampleItems.Create;
using Starter.Application.Tests.Setup;
using Starter.Domain.SampleItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Starter.Application.Tests.SampleItems.Create;

[Collection(IntegrationTestCollection.Name)]
public sealed class CreateSampleItemHandlerTests(PostgreSqlFixture fixture) : ApplicationIntegrationTest(fixture)
{
    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public async Task HandleAsync_ShouldPersistSampleItem(string name, string codeValue)
    {
        var command = new CreateSampleItemCommand(name, new SampleItemCode(codeValue));

        var handler = new CreateSampleItemHandler(_dbContext, NullLogger<CreateSampleItemHandler>.Instance);

        var result = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var sampleItem = await _dbContext.SampleItems.SingleAsync(x => x.Id == result.Value.Id);

        Assert.Equal(name, sampleItem.Name);
        Assert.Equal(command.Code, sampleItem.Code);
    }

    [Theory]
    [InlineData("Acme Bank", "ACME")]
    [InlineData("Global Finance", "GLOBAL")]
    public async Task HandleAsync_WhenCodeAlreadyExists_ShouldReturnFailure(string name, string codeValue)
    {
        var command = new CreateSampleItemCommand(name, new SampleItemCode(codeValue));

        var handler = new CreateSampleItemHandler(_dbContext, NullLogger<CreateSampleItemHandler>.Instance);

        var firstResult = await handler.HandleAsync(command, CancellationToken.None);
        var secondResult = await handler.HandleAsync(command, CancellationToken.None);

        Assert.True(firstResult.IsSuccess);
        Assert.True(secondResult.IsFailure);
        Assert.Equal(SampleItemErrors.AlreadyExists, secondResult.Error);
    }
}
